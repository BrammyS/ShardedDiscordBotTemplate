using System.Threading.Tasks;
using Discord.Commands;

namespace Bot.Interfaces.Discord.Services
{
    public interface ISpamFilter
    {


        /// <summary>
        /// Stops users from spamming commands.
        /// </summary>
        /// <param name="context">The <see cref="SocketCommandContext"/> of the command.</param>
        /// <returns>An awaitable task that returns a <see cref="bool"/>.</returns>
        Task<bool> FilterAsync(SocketCommandContext context);

    }
}
