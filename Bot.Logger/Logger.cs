using System;
using System.IO;
using Bot.Logger.Interfaces;

namespace Bot.Logger
{
    public class Logger : ILogger
    {
        public void Log(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"{DateTime.Now:hh:mm:ss.fff} : " + message);
            Console.ResetColor();
            Log("Misc", message);
        }

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
