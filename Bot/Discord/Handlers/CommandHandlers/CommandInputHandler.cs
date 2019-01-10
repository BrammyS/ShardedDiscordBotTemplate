using System.Linq;
using System.Threading.Tasks;
using Bot.Interfaces.Discord.EventHandlers.CommandHandlers;
using Bot.Logger.Interfaces;
using Discord;
using Discord.Commands;

namespace Bot.Discord.Handlers.CommandHandlers
{
    public class CommandInputErrorHandler : ErrorHandler, ICommandInputErrorHandler
    {
        public CommandInputErrorHandler(ILogger logger) : base(logger)
        {
        }

        public async Task<EmbedBuilder> HandleErrorsAsync(IResult iResult, SocketCommandContext context)
        {
            var message = context.Message.Content.ToLower();
            var result = iResult.ToString();

            if (result.Contains("UnmetPrecondition: Bot requires guild permission EmbedLinks"))
            {
                await context.Channel.SendMessageAsync("I need the EmbedLinks permission for that command!").ConfigureAwait(false);
                return null;
            }

            //User permission errors
            if (result.Contains("UnmetPrecondition: User requires guild permission Administrator"))
                return EmbedError("Permission error", ":no_entry_sign: You need admin permissions for this command.");
            if (result.Contains("UnmetPrecondition: User requires guild permission ManageRoles"))
                return EmbedError("Permission error", ":no_entry_sign: You need Manage Roles permissions for this command.");

            //Sending the user a DM if the bot cant send a message in that channel
            if (result.Contains("UnmetPrecondition: Bot requires guild permission SendMessages"))
            {
                await context.User.SendMessageAsync("", false, EmbedError("Permission error", "I need don't have the right permissions to talk in that channel!\n" +
                                                                                              "Pls give me the SendMessages permission!").Build()).ConfigureAwait(false);
                return null;
            }

            if (result.Contains("A quoted parameter is incomplete"))
                return EmbedError("Incorrect input", "If your are using quotes pls remember to close them <3");

            if (result.Contains("The server responded with error 50013: Missing Permissions") || result.Contains("UnmetPrecondition: Bot requires guild permission"))
            {
                if (context is SocketCommandContext commandContext)
                {
                    var guildPermissions = commandContext.Guild.CurrentUser.Roles.Select(x => x.Permissions).ToList();
                    var description = "Im missing the following permissions:";
                    if (!guildPermissions.Any(x => x.SendMessages)) description += " **SendMessages**";
                    if (!guildPermissions.Any(x => x.EmbedLinks)) description += " **EmbedLinks**";
                    if (!guildPermissions.Any(x => x.AddReactions)) description += " **AddReactions**";
                    await commandContext.Channel.SendMessageAsync(description).ConfigureAwait(false);
                    return null;
                }
            }

            if (result.Contains("BadArgCount: The input text has too many parameters."))
                return EmbedError("Incorrect input", "The input has to many parameters");
            if (result.Contains("BadArgCount: The input text has too few parameters."))
                return EmbedError("Incorrect input", "The input text has too few parameters.");

            return GetDefaultError(result);
        }
    }
}