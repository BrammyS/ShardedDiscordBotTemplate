namespace Bot.Discord.Helpers
{
    public static class ShardStatusTypeEmoji
    {
        public static string GetStatusEmoji(int latency)
        {
            if (latency < 200) return "<:GreenStatus:533010751229526032>";
            if (latency > 200 && latency < 500) return "<:OrangeStatus:533010753196654600>";
            return "<:RedStatus:533010751074467851>";
        }
    }
}
