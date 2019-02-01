using Bot.Persistence.Domain;
using Bot.Persistence.Repositories;

namespace Bot.Persistence.EntityFrameWork.Repositories
{
    public class RequestsRepository : Repository<Request>, IRequestRepository
    {
        public RequestsRepository(BotContext context) : base(context)
        {
        }
    }
}


