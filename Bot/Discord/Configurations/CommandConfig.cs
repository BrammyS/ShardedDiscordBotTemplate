using Discord;
using Discord.Commands;

namespace Bot.Discord.Configurations
{
    public static class CommandConfig
    {


        /// <summary>
        /// Sets the default command service configuration.
        /// </summary>
        /// <returns>The default command service configuration.</returns>
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
