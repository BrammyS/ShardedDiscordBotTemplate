using System;
using System.Threading.Tasks;
using Bot.Persistence.Domain;
using Bot.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bot.Persistence.EntityFrameWork.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BotContext context) : base(context)
        {
        }


        /// <inheritdoc/>
        public async Task<User> GetOrAddUserAsync(ulong id, string userName)
        {
            try
            {
                var exists = await Context.Set<User>().AnyAsync(x => x.Id == id).ConfigureAwait(false);

                // Return the user if it exists in the database.
                // Create a new one if it doesn't exist.
                if (exists) return await Context.Set<User>().FirstOrDefaultAsync(x=>x.Id == id).ConfigureAwait(false);
                var user = await Context.Set<User>().AddAsync(new User
                {
                    Id = id,
                    Name = userName,
                    CommandSpam = 0,
                    CommandUsed = DateTime.Now.AddMinutes(-1),
                    SpamWarning = 0,
                    TotalTimesTimedOut = 0
                    
                }).ConfigureAwait(false);
                await Context.SaveChangesAsync().ConfigureAwait(false);
                return user.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

