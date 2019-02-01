namespace Bot.Persistence.Models
{
    public class ServerPrefix
    {


        /// <summary>The Id of the server.</summary>
        public ulong ServerId { get; set; }


        /// <summary>
        /// The Prefix that the server is using.
        /// This is a <see cref="string.Empty"/> or null if the server isn't using a custom prefix.
        /// </summary>
        public string Prefix { get; set; }
    }
}

