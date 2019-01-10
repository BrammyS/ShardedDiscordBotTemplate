using System;

namespace Bot.Logger.Interfaces
{
    public interface ILogger
    {
        void Log(string message, ConsoleColor color = ConsoleColor.Gray);
        void Log(string folder, string errorReason, string message, string username, string guildName, ulong guildId);
        void Log(string folder, string text);
    }
}