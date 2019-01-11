using System;

namespace Bot.Logger.Interfaces
{
    public interface ILogger
    {

        /// <summary>
        /// Logs a message to the console and logs the message to the Misc logs folder.
        /// </summary>
        /// <param name="message">The message that the user typed.</param>
        /// <param name="color">The color that the message will have when printing something to the console. Default = gray.</param>
        void Log(string message, ConsoleColor color = ConsoleColor.Gray);

        /// <summary>
        /// Logs a command error to text file.
        /// </summary>
        /// <param name="folder">The folder where you want to store the message.</param>
        /// <param name="errorReason">The error reason.</param>
        /// <param name="message">The message that the user typed.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="guildName">The name of the server.</param>
        /// <param name="guildId">The id of the server.</param>
        void Log(string folder, string errorReason, string message, string username, string guildName, ulong guildId);

        /// <summary>
        /// Logs a message to text file.
        /// </summary>
        /// <param name="folder">The folder where you want to store the message.</param>
        /// <param name="text">The text you want to save.</param>
        void Log(string folder, string text);
    }
}
