using Discord;

namespace Bot.Interfaces.Discord.EventHandlers.CommandHandlers
{
    public interface IErrorHandler
    {


        /// <summary>
        /// Gets the default embedded error message.
        /// </summary>
        /// <param name="commandName">The command the user used.</param>
        /// <param name="message">The error message you want to show.</param>
        /// <param name="exception">The exception that will be logged .</param>
        EmbedBuilder GetDefaultError(string commandName, string message, string exception);


        /// <summary>
        /// Gets a embedded error message.
        /// </summary>
        /// <param name="title">The title the message should have.</param>
        /// <param name="description">The description the message will have.</param>
        EmbedBuilder EmbedError(string title, string description);
    }
}
