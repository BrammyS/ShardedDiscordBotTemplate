using Discord;
using Discord.Commands;

namespace Bot.Discord.Configurations
{
    public static class CommandConfig
    {
        public static CommandService GetDefault()
        {
            return new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Info,
                DefaultRunMode = RunMode.Async
            });
        }
    }
}
