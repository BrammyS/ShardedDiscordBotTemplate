using System.Threading.Tasks;
using Bot.Persistence.Repositories;
using Bot.Persistence.UnitOfWorks;

namespace Bot.Persistence.EntityFrameWork.UnitOfWorks
{
    public class ServerUnitOfWork : IServerUnitOfWork
    {
        private readonly BotContext _context;

        public IServerRepository Servers { get; }

        public ServerUnitOfWork(BotContext context, IServerRepository serverRepository)
        {
            _context = context;
            Servers = serverRepository;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

