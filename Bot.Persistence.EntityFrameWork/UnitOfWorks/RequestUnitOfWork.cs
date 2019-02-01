using System.Threading.Tasks;
using Bot.Persistence.Repositories;
using Bot.Persistence.UnitOfWorks;

namespace Bot.Persistence.EntityFrameWork.UnitOfWorks
{
    public class RequestUnitOfWork : IRequestUnitOfWork
    {
        public IRequestRepository Requests { get; private set; }

        private readonly BotContext _context;

        public RequestUnitOfWork(BotContext context, IRequestRepository requestRepository)
        {
            _context = context;
            Requests = requestRepository;
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
