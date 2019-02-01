using System.Threading.Tasks;
using Bot.Persistence.Repositories;
using Bot.Persistence.UnitOfWorks;

namespace Bot.Persistence.EntityFrameWork.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BotContext _context;

        public UnitOfWork(BotContext context, IServerRepository serverRepository, IUserRepository userRepository, IRequestRepository requestRepository)
        {
            _context = context;
            Servers = serverRepository;
            Users = userRepository;
            Requests = requestRepository;
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public IServerRepository Servers { get; }
        public IRequestRepository Requests{ get; }
        public IUserRepository Users { get; }

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

