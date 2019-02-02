using System.Threading.Tasks;
using Bot.Interfaces.Discord.Services;
using Bot.Logger.Interfaces;
using Bot.Persistence.UnitOfWorks;
using Discord;
using Discord.Commands;

namespace Bot.Discord.Commands
{
    public class ServerCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger _logger;
        private readonly EmbedBuilder _embed;


        /// <summary>
        /// Creates a new <see cref="ServerCommands"/>
        /// </summary>
        /// <param name="logger">The logger that will log messages to the console.</param>
        public ServerCommands(ILogger logger)
        {
            _logger = logger;
            _embed = new EmbedBuilder();
        }


        /// <summary>
        /// Changes the prefix for the server.
        /// </summary>
        /// <param name="prefix">The desired prefix.</param>
        [Command("prefix", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task SetPrefixAsync(string prefix)
        {
            using (var unitOfWork = Unity.Resolve<IServerUnitOfWork>())
            {
                var server = await unitOfWork.Servers
                    .GetOrAddServerAsync(Context.Guild.Id, Context.Guild.Name, Context.Guild.MemberCount)
                    .ConfigureAwait(false);

                var customPrefixService = Unity.Resolve<IPrefixService>();

                if (!prefix.Contains("*") && !prefix.Contains("`") && !prefix.Contains("~"))
                {
                    server.Prefix = prefix;
                    customPrefixService.StorePrefix(prefix, Context.Guild.Id);
                    _embed.WithDescription("Successfully added custom prefix!\n" +
                                           $"Example **{prefix}ping**");
                }
                else
                {
                    _embed.WithDescription("You can't have a custom prefix that contains ** * **or **`** or **~**");
                }

                _embed.WithColor(new Color(255, 255, 255));

                await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
                await unitOfWork.SaveAsync().ConfigureAwait(false);

                _logger.Log($"Server: {Context.Guild}, Id: {Context.Guild.Id} || ShardId: {Context.Client.ShardId} || Channel: {Context.Channel} || User: {Context.User} || Used: prefix");
            }
        }


        /// <summary>
        /// Removes the current custom prefix.
        /// </summary>
        [Command("remove prefix", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task RemovePrefixAsync()
        {
            using (var unitOfWork = Unity.Resolve<IServerUnitOfWork>())
            {
                var server = await unitOfWork.Servers
                    .GetOrAddServerAsync(Context.Guild.Id, Context.Guild.Name, Context.Guild.MemberCount)
                    .ConfigureAwait(false);

                var customPrefixService = Unity.Resolve<IPrefixService>();
                customPrefixService.RemovePrefix(Context.Guild.Id);
                server.Prefix = null;

                _embed.WithDescription("Successfully removed custom prefix.");
                _embed.WithColor(new Color(255, 255, 255));

                await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
                await unitOfWork.SaveAsync().ConfigureAwait(false);

                _logger.Log($"Server: {Context.Guild}, Id: {Context.Guild.Id} || ShardId: {Context.Client.ShardId} || Channel: {Context.Channel} || User: {Context.User} || Used: remove prefix");
            }
        }
    }
}

