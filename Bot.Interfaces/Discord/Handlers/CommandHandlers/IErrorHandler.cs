using Discord;

namespace Bot.Interfaces.Discord.Handlers.CommandHandlers
{
    public interface IErrorHandler
    {


        /// <summary>
        /// Embeds the default embedded error message.
        /// </summary>
        /// <param name="commandName">The command the user used.</param>
        /// <param name="message">The error message you want to show.</param>
        /// <param name="exception">The exception that will be logged .</param>
        /// <returns>The embedded error message.</returns>
        EmbedBuilder GetDefaultError(string commandName, string message, string exception);


        /// <summary>
        /// Embeds a embedded error message.
        /// </summary>
        /// <param name="title">The title the message should have.</param>
        /// <param name="description">The description the message will have.</param>
        /// <returns>The embedded error message.</returns>
        EmbedBuilder EmbedError(string title, string description);


        /// <summary>
        /// Embeds the default embedded error message.
        /// </summary>
        /// <param name="exception">The exception that will be logged .</param>
        /// <returns>The embedded error message.</returns>
        EmbedBuilder GetDefaultError(string exception);
    }
}
