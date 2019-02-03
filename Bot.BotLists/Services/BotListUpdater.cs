using System;
using System.Collections.Concurrent;
using System.Net.Cache;
using System.Threading.Tasks;
using Bot.BotLists.Configurations;
using Bot.BotLists.Interfaces.Services;
using Bot.Logger.Interfaces;
using RestSharp;
using RestSharp.Authenticators;

namespace Bot.BotLists.Services
{
    public class BotListUpdater : IBotListUpdater
    {

        private readonly ILogger _logger;


        private static readonly ConcurrentDictionary<string, RestClient> RestClients = new ConcurrentDictionary<string, RestClient>();


        public BotListUpdater(ILogger logger)
        {
            _logger = logger;
            RestClients.TryAdd("DiscordBotsDotOrg", new RestClient
            {
                Authenticator = new JwtAuthenticator(RestClientsConfig.TokenConfig.DiscordBotsDotOrgToken),
                CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore),
                BaseUrl = new Uri("https://discordbots.org/api/")
            });
        }


        /// <inheritdoc />
        public async Task UpdateStatusAsync(ulong botId, int shardCount, int guildCount, int shardId = 0)
        {
            await UpdateDiscordBotsDotOrg(botId, shardCount, guildCount, shardId).ConfigureAwait(false);
        }


        private async Task UpdateDiscordBotsDotOrg(ulong botId, int shardCount, int guildCount, int shardId)
        {

            var request = new RestRequest($"bots/{botId}/stats")
            {
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };
            request.AddOrUpdateParameter("shard_count", shardCount);
            request.AddOrUpdateParameter("server_count", guildCount);
            request.AddOrUpdateParameter("shard_id", shardId);
            var result = await RestClients["DiscordBotsDotOrg"].ExecutePostTaskAsync(request).ConfigureAwait(false);
            if (result.IsSuccessful) return;
            _logger.Log($"Failed to update dblOrg stats reason: {result.ErrorMessage}", ConsoleColor.Red);
        }
    }
}
