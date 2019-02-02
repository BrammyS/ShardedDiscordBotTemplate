using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Persistence.Domain;
using Bot.Persistence.Models;

namespace Bot.Persistence.Repositories
{
    public interface IServerRepository : IRepository<Server>
    {


        /// <summary>
        /// Gets the a server object asynchronously.
        /// With all the includes.
        /// </summary>
        /// <param name="id">The id of the server.</param>
        /// <returns>An awaitable <see cref="Task"/> that returns a <see cref="Server"/>.</returns>
        Task<Server> GetServerAsync(ulong id);


        /// <summary>
        /// Gets the server if it exists in the database.
        /// And it adds the server to the database if it doesn't exist.
        /// </summary>
        /// <param name="id">The id of the server.</param>
        /// <param name="serverName">The name of the server.</param>
        /// <param name="memberCount">The memberCount of the server.</param>
        /// <returns>An awaitable <see cref="Task"/> that returns a <see cref="Server"/>.</returns>
        Task<Server> GetOrAddServerAsync(ulong id, string serverName, int memberCount);


        /// <summary>
        /// Gets all the custom prefixes for all the server.
        /// This should be used when the bot starts up.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/> that returns a <see cref="List{ServerPrefix}"/>.</returns>
        Task<List<ServerPrefix>> GetAllPrefixesAsync();
    }
}

