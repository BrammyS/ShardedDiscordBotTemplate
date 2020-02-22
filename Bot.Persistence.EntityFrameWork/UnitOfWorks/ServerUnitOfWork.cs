using System.Threading.Tasks;
using Bot.Persistence.Repositories;
using Bot.Persistence.UnitOfWorks;

namespace Bot.Persistence.EntityFrameWork.UnitOfWorks
{
    public class ServerUnitOfWork : IServerUnitOfWork
    {
        private readonly BotContext _context;

        public IServerRepository Servers { get; }


        /// <summary>
        /// Creates a new <see cref="ServerUnitOfWork"/>
        /// </summary>
        /// <param name="context">The DbContext that will be used.</param>
        /// <param name="serverRepository">The ServerRepository that will be used.</param>
        public ServerUnitOfWork(BotContext context, IServerRepository serverRepository)
        {
            _context = context;
            Servers = serverRepository;
        }


        /// <inheritdoc/>
        public void Dispose()
        {
            _context.Dispose();
        }


        /// <inheritdoc/>
        public int Save()
        {
            return _context.SaveChanges();
        }


        /// <inheritdoc/>
        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}

