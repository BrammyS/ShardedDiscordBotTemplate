using Bot.Persistence.Domain;
using Bot.Persistence.Repositories;

namespace Bot.Persistence.EntityFrameWork.Repositories
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        public RequestRepository(BotContext context) : base(context)
        {
        }
    }
}


