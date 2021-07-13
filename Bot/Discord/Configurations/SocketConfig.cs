using Discord;
using Discord.WebSocket;

namespace Bot.Discord.Configurations
{
    public static class SocketConfig
    {
        /// <summary>
        ///     Sets the default discord socket configuration.
        /// </summary>
        /// <returns>The default discord socket configuration.</returns>
        public static DiscordSocketConfig GetDefault()
        {
            return new()
            {
                LogLevel = LogSeverity.Info,
                AlwaysDownloadUsers = false,
                MessageCacheSize = 0
            };
        }
    }
}