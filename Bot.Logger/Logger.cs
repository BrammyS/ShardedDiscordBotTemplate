using System;
using System.IO;
using Bot.Logger.Interfaces;

namespace Bot.Logger
{
    public class Logger : ILogger
    {
        /// <inheritdoc />
        /// <summary>
        /// Logs a message to the console and logs the message to the Misc logs folder.
        /// </summary>
        /// <param name="message">The message that the user typed.</param>
        /// <param name="color">The color that the message will have when printing something to the console. Default = gray.</param>
        public void Log(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"{DateTime.Now:hh:mm:ss.fff} : " + message);
            Console.ResetColor();
            Log("Misc", message);
        }


        /// <inheritdoc />
        /// <summary>
        /// Logs a message to text file.
        /// </summary>
        /// <param name="folder">The folder where you want to store the message.</param>
        /// <param name="text">The text you want to save.</param>
        public void Log(string folder, string text)
        {
            var filePath = $"Logs/{folder}/{DateTime.Now:MMMM, yyyy}";
            if (!File.Exists(filePath)) Directory.CreateDirectory(filePath);
            filePath += $"/{DateTime.Now:dddd, MMMM d, yyyy}.txt";
            using (var file = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                using (var sw = new StreamWriter(file))
                {
                    sw.WriteLine($"{DateTime.Now:T} : {text}");
                }
            }
        }


        /// <inheritdoc />
        /// <summary>
        /// Logs a command error to text file.
        /// </summary>
        /// <param name="folder">The folder where you want to store the message.</param>
        /// <param name="errorReason">The error reason.</param>
        /// <param name="message">The message that the user typed.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="guildName">The name of the server.</param>
        /// <param name="guildId">The id of the server.</param>
        public void Log(string folder, string errorReason, string message, string username, string guildName, ulong guildId)
        {
            var filePath = $"Logs/{folder}/{DateTime.Now:MMMM, yyyy}";
            if (!File.Exists(filePath)) Directory.CreateDirectory(filePath);

            filePath += $"/{DateTime.Now:dddd, MMMM d, yyyy}.txt";
            using (var file = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                using (var sw = new StreamWriter(file))
                {
                    sw.WriteLine($"{DateTime.Now:T} : {errorReason}");
                    sw.WriteLine($"{DateTime.Now:T} : {message}");
                    sw.WriteLine($"{DateTime.Now:T} : User: {username}");
                    sw.WriteLine($"{DateTime.Now:T} : Guild: {guildName} Id: {guildId}");
                    sw.WriteLine("===========================================================");
                }
            }
        }
    }
}
