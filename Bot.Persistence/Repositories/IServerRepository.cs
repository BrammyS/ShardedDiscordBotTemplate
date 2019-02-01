using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Persistence.Domain;
using Bot.Persistence.Models;

namespace Bot.Persistence.Repositories
{
    public interface IServerRepository : IRepository<Server>
    {
        Task<Server> GetServerAsync(long id);
        Task<Server> GetOrAddServer(long id, string serverName, int memberCount);
        Task<List<ServerPrefix>> GetAllPrefixesAsync();
    }
}

