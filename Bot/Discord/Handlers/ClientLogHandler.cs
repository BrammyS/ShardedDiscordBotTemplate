using System;
using System.Threading.Tasks;
using Bot.Interfaces.Discord.Handlers;
using Bot.Logger.Interfaces;
using Discord;
using Discord.WebSocket;

namespace Bot.Discord.Handlers
{
    public class ClientLogHandler : IClientLogHandler
    {
        private readonly DiscordShardedClient _client;
        private readonly ILogger _logger;


        /// <summary>
        /// Creates a new <see cref="ClientLogHandler"/>.
        /// </summary>
        /// <param name="client">The <see cref="DiscordShardedClient"/> that will be used.</param>
        /// <param name="logger">The <see cref="ILogger"/> that will be used to log all the messages.</param>
        public ClientLogHandler(DiscordShardedClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }


        /// <inheritdoc />
        public void Initialize()
        {
            _client.Log += LogEvent;
            _client.ShardLatencyUpdated += ShardLatencyEvent;
            _client.ShardDisconnected += ShardDisconnectedEvent;
            _client.ShardConnected += ShardConnectedEvent;
            _client.JoinedGuild += ClientJoinedGuildEvent;
        }

        /// <summary>
        /// Handles the <see cref="BaseSocketClient.JoinedGuild"/> event.
        /// </summary>
        /// <param name="guild">The server that the client joined.</param>
        private Task ClientJoinedGuildEvent(SocketGuild guild)
        {
            Task.Run(async () => await ClientJoinedGuild(guild).ConfigureAwait(false));
            return Task.CompletedTask;
        }


        /// <summary>
        /// Handles the <see cref="DiscordShardedClient.Log"/> event.
        /// </summary>
        /// <param name="logMessage">The log message.</param>
        private Task LogEvent(LogMessage logMessage)
        {
            Task.Run(() =>
            {
                // If log message is a Serializer Error, Log the message to the SerializerError folder.
                if (logMessage.Message.Contains("Serializer Error")) _logger.Log("SerializerError", $"Source: {logMessage.Source} Exception: {logMessage.Exception} Message: {logMessage.Message}");
                Log(logMessage.Message);
            });
            return Task.CompletedTask;
        }


        /// <summary>
        /// Logs a string to the console.
        /// </summary>
        /// <param name="message">The message that will be logged.</param>
        private void Log(string message)
        {
            if (message.Contains("Unknown User") || message.Contains("Unknown Guild"))
            {
                _logger.Log("Unknown", message);
                return;
            }
            _logger.Log(message);
        }


        /// <summary>
        /// Handles the <see cref="DiscordShardedClient.ShardDisconnected"/> event.
        /// </summary>
        /// <param name="exception">The exception of the disconnected shard.</param>
        /// <param name="shard">The shard that got disconnected.</param>
        private Task ShardDisconnectedEvent(Exception exception, DiscordSocketClient shard)
        {
            Task.Run(async () => await ShardDisconnectedAsync(exception, shard).ConfigureAwait(false));
            return Task.CompletedTask;
        }


        /// <summary>
        /// Handles the <see cref="DiscordShardedClient.ShardLatencyUpdated"/> event.
        /// </summary>
        /// <param name="oldPing">The latency value before it was updated.</param>
        /// <param name="updatePing">The new latency value</param>
        /// <param name="shard">The shard that got disconnected.</param>
        private Task ShardLatencyEvent(int oldPing, int updatePing, DiscordSocketClient shard)
        {
            Task.Run(async () => await ShardLatencyUpdatedAsync(oldPing, updatePing, shard).ConfigureAwait(false));
            return Task.CompletedTask;
        }


        /// <summary>
        /// Handles the <see cref="DiscordShardedClient.ShardConnected"/> event.
        /// </summary>
        /// <param name="shard">The shard that got connected.</param>
        private Task ShardConnectedEvent(DiscordSocketClient shard)
        {
            Task.Run(async () => await ShardConnectedAsync(shard).ConfigureAwait(false));
            return Task.CompletedTask;
        }


        /// <summary>
        /// Sends a message that a shard disconnected
        /// </summary>
        /// <param name="exception">The exception of the disconnected shard.</param>
        /// <param name="shard">The shard that got disconnected.</param>
        private async Task ShardDisconnectedAsync(Exception exception, DiscordSocketClient shard)
        {

            try
            {
                var channel = _client.GetGuild(Constants.EventSeverId).GetTextChannel(Constants.DisconnectEventChannelId);
                await channel.SendMessageAsync($"<:RedStatus:519932993343586350> Shard: `{shard.ShardId}` Disconnected with the reason {exception.Message}").ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Logs the message to a txt file if it was unable to send the message.
                _logger.Log("Connection/Disconnected", $"Shard: {shard.ShardId} reason: {exception.Message}");
            }
        }


        /// <summary>
        /// Sends a message that a shard connected.
        /// </summary>
        /// <param name="shard">The shard that got connected.</param>
        private async Task ShardConnectedAsync(DiscordSocketClient shard)
        {

            try
            {
                await Task.Delay(30 * 1000).ConfigureAwait(false);
                var channel = _client.GetGuild(Constants.EventSeverId).GetTextChannel(Constants.ConnectEventChannelId);
                await channel.SendMessageAsync($"<:GreenStatus:519932750296514605> Shard: `{shard.ShardId}` Connected with {shard.Latency}ms").ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Logs the message to a txt file if it was unable to send the message.
                _logger.Log("Connection/Connected", $"Shard: {shard.ShardId} with **{shard.Latency}** ms");
            }
        }


        /// <summary>
        /// Sends a message that a shard latency was updated.
        /// </summary>
        /// <param name="oldPing">The latency value before it was updated.</param>
        /// <param name="updatePing">The new latency value.</param>
        /// <param name="shard">The shard that got disconnected.</param>
        private async Task ShardLatencyUpdatedAsync(int oldPing, int updatePing, DiscordSocketClient shard)
        {

            // If new or old latency if lager then 500ms.
            if (updatePing < 500 && oldPing < 500) return;
            try
            {
                var channel = _client.GetGuild(Constants.EventSeverId).GetTextChannel(Constants.LatencyUpdatedEventChannelId);
                await channel.SendMessageAsync($"Shard: `{shard.ShardId}` Latency update from **{oldPing}** ms to **{updatePing}** ms").ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Logs the message to a txt file if it was unable to send the message.
                _logger.Log("Connection/Latency", $"Shard: {shard.ShardId} Latency: {updatePing}");
            }
        }


        /// <summary>
        /// Sends a message that the client joined a server.
        /// </summary>
        /// <param name="guild">The server that the client joined.</param>
        private async Task ClientJoinedGuild(SocketGuild guild)
        {

            try
            {
                var channel = _client.GetGuild(Constants.EventSeverId).GetTextChannel(Constants.JoinGuildChannelId);
                await channel.SendMessageAsync($"Joined server: {guild.Name}, Id: {guild.Id}, MemberCount: {guild.MemberCount}.").ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Logs the message to a txt file if it was unable to send the message.
                _logger.Log("Connection/Latency", $"Joined server: {guild.Name}, Id: {guild.Id}, MemberCount: {guild.MemberCount}.");
            }
        }
    }
}
