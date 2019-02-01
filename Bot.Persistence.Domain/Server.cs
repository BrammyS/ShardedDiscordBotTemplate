using System;
using System.Collections.Generic;

namespace Bot.Persistence.Domain
{
    public class Server
    {

        /// <summary>The id of the server.</summary>
        public ulong Id { get; set; }

        /// <summary>The name of the server.</summary>
        public string Name { get; set; }

        /// <summary>
        /// The custom prefix of the server.
        /// This is an empty string or null when the server isn't using a custom prefix.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>The amount of members in the server.</summary>
        public int TotalMembers { get; set; }

        /// <summary>The <see cref="DateTime"/> of when the bot joined this server.</summary>
        public DateTime JoinDate { get; set; }

        /// <summary>
        /// The <see cref="bool"/> of the server to see if the bot is still the server.
        /// This value is True if the bot is this in the server.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>All the <see cref="Requests"/> that were made in the server.</summary>
        public List<Request> Requests { get; set; }
    }
}