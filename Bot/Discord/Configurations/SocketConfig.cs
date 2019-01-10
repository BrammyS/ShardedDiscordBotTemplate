using Discord;
using Discord.WebSocket;

namespace Bot.Discord.Configurations
{
    public static class SocketConfig
    {
        public static DiscordSocketConfig GetDefault()
        {
            return new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                AlwaysDownloadUsers = false,
                MessageCacheSize = 0
            };
        }
    }
}
