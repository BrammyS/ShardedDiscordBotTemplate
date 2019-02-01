using System;
using System.Threading.Tasks;
using Bot.Persistence.Repositories;

namespace Bot.Persistence.UnitOfWorks
{
    public interface IRequestUnitOfWork : IDisposable
    {
        IRequestRepository Requests { get; }
        int Save();
        Task<int> SaveAsync();
    }
}