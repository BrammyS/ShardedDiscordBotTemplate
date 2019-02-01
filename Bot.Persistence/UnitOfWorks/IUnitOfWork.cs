using System;
using System.Threading.Tasks;
using Bot.Persistence.Repositories;

namespace Bot.Persistence.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IServerRepository Servers { get; }
        IUserRepository Users { get; }
        IRequestRepository Requests { get; }
        int Save();
        Task<int> SaveAsync();

    }
}