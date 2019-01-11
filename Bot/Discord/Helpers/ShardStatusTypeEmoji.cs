namespace Bot.Discord.Helpers
{
    public static class ShardStatusTypeEmoji
    {
        /// <summary>
        /// Converts a integer to a status emoji.
        /// </summary>
        /// <param name="latency">The latency of a client or shards.</param>
        /// <returns>
        /// A status emoji as a <see cref="string"/>
        /// </returns>
        public static string GetStatusEmoji(int latency)
        {
            if (latency < 200) return Constants.GreenStatusEmoji;
            if (latency > 200 && latency < 500) return Constants.OrangeStatusEmoji;
            return Constants.RedStatusEmoji;
        }
    }
}
