using System;
using Bot.Interfaces.Discord.Handlers.CommandHandlers;
using Bot.Logger.Interfaces;
using Discord;

namespace Bot.Discord.Handlers.CommandHandlers
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly ILogger _logger;


        /// <summary>
        /// The default error message that will can be shown in the description of an embed.
        /// </summary>
        private const string DefaultErrorMessage =
            "an unexpected error occurred. Pls try again.\n" +
            "Join [UPDATE](https://discord.ggINVITE) discord if nothing changes!";


        /// <summary>
        /// Creates a new <see cref="ErrorHandler"/>.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> that will be used to log all the messages.</param>
        public ErrorHandler(ILogger logger)
        {
            _logger = logger;
        }


        /// <inheritdoc />
        public EmbedBuilder GetDefaultError(string commandName, string message, string exception)
        {
            _logger.Log("UnHandledErrors", $"Command: {commandName} Exception: {message} Exception info: {exception}");
            return EmbedError("Generic error", DefaultErrorMessage);
        }


        /// <inheritdoc />
        public EmbedBuilder GetDefaultError(string result)
        {
            _logger.Log("UnHandledErrors", result);
            return EmbedError("Generic error", DefaultErrorMessage);
        }


        /// <inheritdoc />
        public EmbedBuilder EmbedError(string title, string description)
        {
            return new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    IconUrl = "https://i.gyazo.com/3397330afb79f94f8d494d6d83aa5490.png",
                    Name = title
                },
                Description = description,
                Timestamp = DateTimeOffset.Now,
                Color = Color.DarkRed
            };
        }
    }
}
