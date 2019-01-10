using System.Threading.Tasks;
using Bot.Configurations;
using Bot.Interfaces.Discord.EventHandlers;
using Discord;
using Discord.WebSocket;
using IConnection = Bot.Interfaces.Discord.IConnection;

namespace Bot.Discord
{
    public class Connection : IConnection
    {
        private readonly DiscordShardedClient _client;
        private readonly IClientLogHandler _clientLogHandler;

        public Connection(DiscordShardedClient client, IClientLogHandler clientLogHandler)
        {
            _client = client;
            _clientLogHandler = clientLogHandler;
        }

        public async Task ConnectAsync()
        {
            //Start the connection to discord
            await _client.LoginAsync(TokenType.Bot, ConfigData.Data.Token).ConfigureAwait(false);
            await _client.StartAsync().ConfigureAwait(false);

            //Initialize all the client logging
            _clientLogHandler.Initialize();

            await Task.Delay(ConfigData.Data.RestartTime * 60000).ConfigureAwait(false);
            await _client.StopAsync().ConfigureAwait(false);
        }
    }
}
