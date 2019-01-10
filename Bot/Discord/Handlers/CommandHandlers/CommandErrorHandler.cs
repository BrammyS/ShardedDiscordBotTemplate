using System.Linq;
using System.Threading.Tasks;
using Bot.Interfaces.Discord.EventHandlers.CommandHandlers;
using Bot.Logger.Interfaces;
using Discord;
using Discord.Commands;

namespace Bot.Discord.Handlers.CommandHandlers
{
    public class CommandErrorHandler : ErrorHandler, ICommandErrorHandler
    {
        public CommandErrorHandler(ILogger logger) : base(logger)
        {
        }

        public async Task<EmbedBuilder> HandleErrorsAsync(CommandException commandException)
        {
            var context = commandException.Context;
            var message = context.Message.Content.ToLower();
            var commandName = commandException.Command.Name;
            var exception = commandException.GetBaseException().Message;
            if (exception.Contains("The server responded with error 50013: Missing Permissions"))
            {
                if (context is SocketCommandContext commandContext)
                {
                    var rolePermission = commandContext.Guild.CurrentUser.Roles.Select(x => x.Permissions).ToList();
                    var channelPermissions = commandContext.Guild.CurrentUser.GetPermissions(commandContext.Channel as IGuildChannel);

                    var description = "Im missing the following permissions:";
                    if (!rolePermission.Any(x => x.ManageRoles)) description += " **ManageRoles**";
                    if (!channelPermissions.SendMessages) description += " **SendMessages**";
                    if (!channelPermissions.ViewChannel) description += " **ReadMessages**";
                    if (!channelPermissions.AttachFiles) description += " **AttachFiles**";
                    if (!channelPermissions.EmbedLinks) description += " **EmbedLinks**";
                    if (!channelPermissions.AddReactions) description += " **AddReactions**";
                    if (!channelPermissions.ReadMessageHistory) description += " **ReadMessageHistory**";
                    if (!channelPermissions.ManageMessages) description += " **ManageMessages**";
                    await commandContext.Channel.SendMessageAsync(description).ConfigureAwait(false);
                    return null;
                }
            }
            return GetDefaultError(commandName, message, exception);
        }
    }
}