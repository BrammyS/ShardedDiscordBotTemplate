using System;
using System.Threading.Tasks;
using Bot.Interfaces;

namespace Bot
{
    internal class Program
    {
        private static Task Main()
        {
            Unity.RegisterTypes();
            var bot = Unity.Resolve<IBot>();
            return bot.StartAsync();
        }
    }
}