using System.Threading.Tasks;

namespace Bot.Interfaces
{
    public interface IBot
    {

        /// <summary>
        /// Starts the login process of the bot.
        /// </summary>
        Task StartAsync();
    }
}
