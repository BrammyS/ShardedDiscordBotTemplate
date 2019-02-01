using Bot.Persistence.Domain;
using Bot.Persistence.Repositories;

namespace Bot.Persistence.EntityFrameWork.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BotContext context) : base(context)
        {
        }
    }
}

