namespace Bot.Discord.Helpers
{
    public static class ShardStatusTypeEmoji
    {


        /// <summary>
        /// Converts a integer to a status emoji.
        /// </summary>
        /// <param name="latency">The latency of a client or shards.</param>
        /// <returns>A status emoji.</returns>
        public static string GetStatusEmoji(int latency)
        {
            if (latency < 200) return "<:GreenStatus:533010751229526032>";
            if (latency > 200 && latency < 500) return "<:OrangeStatus:533010753196654600>";
            return "<:RedStatus:533010751074467851>";
        }
    }
}
