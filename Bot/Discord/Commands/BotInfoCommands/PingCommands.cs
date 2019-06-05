using System.Linq;
using System.Threading.Tasks;
using Bot.Discord.Helpers;
using Bot.Logger.Interfaces;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Bot.Discord.Commands.BotInfoCommands
{
    [Name("Ping")]
    public class PingCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger _logger;
        private readonly DiscordShardedClient _shardedClient;
        private readonly EmbedBuilder _embed;

        public PingCommands(ILogger logger, DiscordShardedClient shardedClient)
        {
            _logger = logger;
            _shardedClient = shardedClient;
            _embed = new EmbedBuilder();
        }


        /// <summary>
        /// Sends the ping of the shard that is connected to the server where the command is requested.
        /// </summary>
        [Command("ping", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task PingAsync()
        {
            _embed.WithTitle("Info for" + Context.User.Username);
            _embed.WithDescription($"{Context.Client.Latency} ms");
            _embed.WithColor(new Color(255, 255, 255));
            await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
            _logger.LogCommandUsed(Context.Guild?.Id, Context.Client.ShardId, Context.Channel.Id, Context.User.Id, "Ping");
        }


        /// <summary>
        /// Sends the ping of all shards to the server where the command is requested.
        /// </summary>
        [Command("shards", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task ShardsAsync()
        {
            _embed.WithTitle("Shard info for " + Context.User.Username);
            foreach (var shard in _shardedClient.Shards)
            {
                _embed.AddField($"Shard: {shard.ShardId} {ShardStatusTypeEmoji.GetStatusEmoji(shard.Latency)}", $"{shard.Latency} ms\n" +
                                                                                                                $"{shard.Guilds.Count} Servers\n" +
                                                                                                                $"{shard.Guilds.Sum(x => x.MemberCount)} Members", true);
            }
            _embed.WithDescription($"Average ping: {_shardedClient.Shards.Average(x => x.Latency)} ms");
            _embed.WithFooter($"You are on shard: {Context.Client.ShardId}");
            _embed.WithColor(new Color(255, 255, 255));
            await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
            _logger.LogCommandUsed(Context.Guild?.Id, Context.Client.ShardId, Context.Channel.Id, Context.User.Id, "Shards");
        }
    }
}
