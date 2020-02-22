using System.Threading.Tasks;
using Bot.Persistence.Repositories;
using Bot.Persistence.UnitOfWorks;

namespace Bot.Persistence.EntityFrameWork.UnitOfWorks
{
    public class RequestUnitOfWork : IRequestUnitOfWork
    {
        public IRequestRepository Requests { get; private set; }

        private readonly BotContext _context;


        /// <summary>
        /// Creates a new <see cref="RequestUnitOfWork"/>
        /// </summary>
        /// <param name="context">The DbContext that will be used.</param>
        /// <param name="requestRepository">The RequestRepository that will be used.</param>
        public RequestUnitOfWork(BotContext context, IRequestRepository requestRepository)
        {
            _context = context;
            Requests = requestRepository;
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
