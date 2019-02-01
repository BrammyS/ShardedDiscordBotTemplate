using System;
using System.Threading.Tasks;
using Bot.Persistence.Repositories;

namespace Bot.Persistence.UnitOfWorks
{
    public interface IServerUnitOfWork : IDisposable
    {
        IServerRepository Servers { get; }
        int Save();
        Task<int> SaveAsync();
    }
}
