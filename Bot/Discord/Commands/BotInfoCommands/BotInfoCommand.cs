using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bot.Logger.Interfaces;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Bot.Discord.Commands.BotInfoCommands
{
    [Name("BotInfo")]
    public class BotInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger _logger;
        private readonly DiscordShardedClient _shardedClient;
        private readonly EmbedBuilder _embed;
        public BotInfoCommand(ILogger logger, DiscordShardedClient shardedClient)
        {
            _logger = logger;
            _shardedClient = shardedClient;
            _embed = new EmbedBuilder();
        }


        /// <summary>
        /// Sends bot info about the current client.
        /// </summary>
        [Command("BotInfo", RunMode = RunMode.Async)]
        public async Task StatsAsync()
        {
            var ramUsages = Math.Round((decimal)Process.GetCurrentProcess().PrivateMemorySize64 / 1000000000, 2);
            var upTime = DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime);
            var upTimeString = $"{upTime.Days}D:{upTime.Hours}H:{upTime.Minutes}M:{upTime.Seconds}S";
            _embed.WithThumbnailUrl(_shardedClient.CurrentUser.GetAvatarUrl());
            _embed.WithTitle("Bot info for " + Context.User.Username);
            _embed.AddField("Bot:", $"{_shardedClient.CurrentUser.Username}#{_shardedClient.CurrentUser.Discriminator}", true);
            _embed.AddField("Bot id:", _shardedClient.CurrentUser.Id, true);
            _embed.AddField("Owner:", Constants.OwnerUsername, true);
            _embed.AddField("Owner id:", Constants.OwnerId, true);
            _embed.AddField("RAM Usage:", $"{ramUsages}GB", true);
            _embed.AddField("Shards:", _shardedClient.Shards.Count, true);
            _embed.AddField("Servers:", _shardedClient.Shards.Sum(x => x.Guilds.Count), true);
            _embed.AddField("Members:", _shardedClient.Shards.SelectMany(shard => shard.Guilds).Sum(guild => guild.MemberCount), true);
            _embed.AddField("Avg ping:", _shardedClient.Shards.Average(x => x.Latency), true);
            _embed.AddField("Up time:", upTimeString, true);
            _embed.WithColor(new Color(255, 255, 255));
            _embed.WithCurrentTimestamp();
            await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
            _logger.LogCommandUsed(Context.Guild?.Id, Context.Client.ShardId, Context.Channel.Id, Context.User.Id, "BotInfo");
        }
    }
}
