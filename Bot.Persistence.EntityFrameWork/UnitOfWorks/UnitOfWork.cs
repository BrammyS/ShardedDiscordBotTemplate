using System.Threading.Tasks;
using Bot.Persistence.Repositories;
using Bot.Persistence.UnitOfWorks;

namespace Bot.Persistence.EntityFrameWork.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BotContext _context;


        /// <summary>
        /// Creates a new <see cref="UnitOfWork"/>
        /// </summary>
        /// <param name="context">The DbContext that will be used.</param>
        /// <param name="serverRepository">The ServerRepository that will be used.</param>
        /// <param name="userRepository">The UserRepository that will be used.</param>
        /// <param name="requestRepository">The RequestRepository that will be used.</param>
        public UnitOfWork(BotContext context, IServerRepository serverRepository, IUserRepository userRepository, IRequestRepository requestRepository)
        {
            _context = context;
            Servers = serverRepository;
            Users = userRepository;
            Requests = requestRepository;
        }

        /// <inheritdoc/>
        public IServerRepository Servers { get; }

        /// <inheritdoc/>
        public IRequestRepository Requests{ get; }

        /// <inheritdoc/>
        public IUserRepository Users { get; }


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


        /// <inheritdoc/>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

