using System;
using System.Threading.Tasks;
using Bot.Interfaces.Discord.EventHandlers;
using Bot.Logger.Interfaces;
using Discord;
using Discord.WebSocket;

namespace Bot.Discord.EventHandlers
{
    public class ClientLogHandler : IClientLogHandler
    {
        private readonly DiscordShardedClient _client;
        private readonly ILogger _logger;

        public ClientLogHandler(DiscordShardedClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public void Initialize()
        {
            _client.Log += LogEvent;
            _client.ShardLatencyUpdated += ShardLatencyEvent;
            _client.ShardDisconnected += ShardDisconnectedEvent;
            _client.ShardConnected += ShardConnectedEvent;
        }

        private Task LogEvent(LogMessage logMessage)
        {
            Task.Run(() =>
            {
                if (logMessage.Message.Contains("Serializer Error")) _logger.Log("SerializerError", $"Source: {logMessage.Source} Exception: {logMessage.Exception} Message: {logMessage.Message}");
                Log(logMessage.Message);
            });
            return Task.CompletedTask;
        }

        private void Log(string message)
        {
            if (message.Contains("Unknown User") || message.Contains("Unknown Guild"))
            {
                _logger.Log("Unknown", message);
                return;
            }
            _logger.Log(message);
        }

        private Task ShardDisconnectedEvent(Exception exception, DiscordSocketClient shard)
        {
            Task.Run(async () => await ShardDisconnectedAsync(exception, shard).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        private Task ShardLatencyEvent(int oldPing, int updatePing, DiscordSocketClient shard)
        {
            Task.Run(async () => await ShardLatencyUpdatedAsync(oldPing, updatePing, shard).ConfigureAwait(false));
            return Task.CompletedTask;
        }
        private Task ShardConnectedEvent(DiscordSocketClient socketClient)
        {
            Task.Run(async () => await ShardConnectedAsync(socketClient).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        private async Task ShardDisconnectedAsync(Exception exception, DiscordSocketClient shard)
        {
            try
            {
                var channel = _client.GetGuild(403577303784882186).GetTextChannel(512966928491479040);
                await channel.SendMessageAsync($"<:RedStatus:519932993343586350> Shard: `{shard.ShardId}` Disconnected with the reason {exception.Message}").ConfigureAwait(false);
            }
            catch (Exception)
            {
                _logger.Log("Connection/Disconnected", $"Shard: {shard.ShardId} reason: {exception.Message}");
            }
        }

        private async Task ShardConnectedAsync(DiscordSocketClient shard)
        {
            try
            {
                await Task.Delay(30 * 1000).ConfigureAwait(false);
                var channel = _client.GetGuild(403577303784882186).GetTextChannel(512966928491479040);
                await channel.SendMessageAsync($"<:GreenStatus:519932750296514605> Shard: `{shard.ShardId}` Connected with {shard.Latency}ms").ConfigureAwait(false);
            }
            catch (Exception)
            {
                _logger.Log("Connection/Connected", $"Shard: {shard.ShardId} with **{shard.Latency}** ms");
            }
        }

        private async Task ShardLatencyUpdatedAsync(int oldPing, int updatePing, DiscordSocketClient shard)
        {
            if (updatePing < 1000 && oldPing < 1000) return;
            try
            {
                var channel = _client.GetGuild(403577303784882186).GetTextChannel(520276841970401280);
                await channel.SendMessageAsync($"Shard: `{shard.ShardId}` Latency update from **{oldPing}** ms to **{updatePing}** ms").ConfigureAwait(false);
            }
            catch (Exception)
            {
                _logger.Log("Connection/Latency", $"Shard: {shard.ShardId} Latency: {updatePing}");
            }
        }
    }
}
