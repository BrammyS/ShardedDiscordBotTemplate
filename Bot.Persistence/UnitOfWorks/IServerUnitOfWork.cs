using System;
using System.Threading.Tasks;
using Bot.Persistence.Repositories;

namespace Bot.Persistence.UnitOfWorks
{
    public interface IServerUnitOfWork : IDisposable
    {

        /// <summary>
        /// The table containing all the Servers.
        /// </summary>

        IServerRepository Servers { get; }


        /// <summary>
        /// Saves all the changes in the context to the database.
        /// </summary>
        /// <returns>Returns the amount of changes it saved.</returns>
        int Save();


        /// <summary>
        /// Saves all the changes in the context to the database asynchronously.
        /// </summary>
        /// <returns>Returns the amount of changes it saved.</returns>
        Task<int> SaveAsync();
    }
}
