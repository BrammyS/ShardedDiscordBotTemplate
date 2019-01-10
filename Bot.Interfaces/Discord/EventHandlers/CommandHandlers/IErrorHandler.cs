using Discord;

namespace Bot.Interfaces.Discord.EventHandlers.CommandHandlers
{
    public interface IErrorHandler
    {
        EmbedBuilder GetDefaultError(string commandName, string message, string exception);
        EmbedBuilder EmbedError(string title, string description);
    }
}
