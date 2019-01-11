using System.Threading.Tasks;

namespace Bot.Interfaces.Discord
{
    public interface IConnection
    {

        /// <summary>
        /// Starts the connection to discord.
        /// </summary>
        Task ConnectAsync();
    }
}
