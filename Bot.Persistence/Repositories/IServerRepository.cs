using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Persistence.Domain;
using Bot.Persistence.Models;

namespace Bot.Persistence.Repositories
{
    public interface IServerRepository : IRepository<Server>
    {
        Task<Server> GetServerAsync(ulong id);
        Task<Server> GetOrAddServer(ulong id, string serverName, int memberCount);
        Task<List<ServerPrefix>> GetAllPrefixesAsync();
    }
}

