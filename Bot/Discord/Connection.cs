using System.Threading.Tasks;
using Bot.Configurations;
using Bot.Interfaces.Discord.EventHandlers;
using Bot.Interfaces.Discord.EventHandlers.CommandHandlers;
using Discord;
using Discord.WebSocket;
using IConnection = Bot.Interfaces.Discord.IConnection;

namespace Bot.Discord
{
    public class Connection : IConnection
    {
        private readonly DiscordShardedClient _client;
        private readonly IClientLogHandler _clientLogHandler;
        private readonly ICommandHandler _commandHandler;

        public Connection(DiscordShardedClient client, IClientLogHandler clientLogHandler, ICommandHandler commandHandler)
        {
            _client = client;
            _clientLogHandler = clientLogHandler;
            _commandHandler = commandHandler;
        }

        public async Task ConnectAsync()
        {
            //Start the connection to discord
            await _client.LoginAsync(TokenType.Bot, ConfigData.Data.Token).ConfigureAwait(false);
            await _client.StartAsync().ConfigureAwait(false);

            //Initialize all the client logging
            _clientLogHandler.Initialize();

            await _commandHandler.InitializeAsync().ConfigureAwait(false);

            await Task.Delay(ConfigData.Data.RestartTime * 60000).ConfigureAwait(false);
            await _client.StopAsync().ConfigureAwait(false);
        }
    }
}
