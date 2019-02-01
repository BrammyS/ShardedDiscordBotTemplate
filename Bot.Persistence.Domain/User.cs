using System;
using System.Collections.Generic;

namespace Bot.Persistence.Domain
{
    public class User
    {

        /// <summary>The id of the user.</summary>
        public ulong Id { get; set; }

        /// <summary>The username of the user.</summary>
        public string Name { get; set; }

        /// <summary>The <see cref="DateTime"/> of when the user used a command for the last time.</summary>
        public DateTime CommandUsed { get; set; }

        /// <summary>The amount of times the user was timed out for using commands to fast.</summary>
        public int TotalTimesTimedOut { get; set; }

        /// <summary>The amount of warning the user received when the use is using commands to fast.</summary>
        public int SpamWarning { get; set; }

        /// <summary>The amount of times the user was using commands to fast.</summary>
        public int CommandSpam { get; set; }

        /// <summary>All the <see cref="Requests"/> that were made by the user.</summary>
        public List<Request> Requests { get; set; }
    }
}
