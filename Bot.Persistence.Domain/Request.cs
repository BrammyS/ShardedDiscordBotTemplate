using System;

namespace Bot.Persistence.Domain
{
    public class Request
    {

        /// <summary>The id of the request.</summary>
        public long Id { get; set; }

        /// <summary>The command that was requested.</summary>
        public string Command { get; set; }

        /// <summary>
        /// The error message of the request.
        /// This value is Null if <see cref="IsSuccessFull"/> is True.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>The success value for the command request. This is True if it was success full.</summary>
        public bool IsSuccessFull { get; set; }

        /// <summary>The <see cref="DateTime"/> value of when the command was used.</summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>The amount of milliseconds it took to active the command form when it received the message.</summary>
        public long RunTime { get; set; }

        /// <summary>The id of the server where the command was used.</summary>
        public ulong? ServerId { get; set; }

        /// <summary>The server where the command was used.</summary>
        public Server Server { get; set; }

        /// <summary>The id of the user that used the command.</summary>
        public ulong UserId { get; set; }

        /// <summary>The user that used the command.</summary>
        public User User { get; set; }
    }
}
