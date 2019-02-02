using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Persistence.Domain;
using Bot.Persistence.Models;
using Bot.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bot.Persistence.EntityFrameWork.Repositories
{
    public class ServerRepository : Repository<Server>, IServerRepository
    {
        public ServerRepository(BotContext context) : base(context)
        {
        }


        /// <inheritdoc/>
        public async Task<Server> GetServerAsync(ulong id)
        {
            try
            {
                return await Context.Set<Server>()
                                    //.Include(x => x.Requests)
                                    .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        /// <inheritdoc/>
        public async Task<List<ServerPrefix>> GetAllPrefixesAsync()
        {
            try
            {
                return
                    await (from server in Context.Set<Server>()
                           where server.Prefix != null
                           select new ServerPrefix
                           {
                               ServerId = server.Id,
                               Prefix = server.Prefix
                           }).ToListAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        /// <inheritdoc/>
        public async Task<Server> GetOrAddServerAsync(ulong id, string serverName, int memberCount)
        {
            try
            {
                var exists = await Context.Set<Server>().AnyAsync(x => x.Id == id).ConfigureAwait(false);

                // Return the server if it exists in the database.
                // Create a new one if it doesn't exist.
                if (exists) return await GetServerAsync(id).ConfigureAwait(false);
                var server = await Context.Set<Server>().AddAsync(new Server
                {
                    Id = id,
                    Name = serverName,
                    Active = true,
                    Prefix = null,
                    JoinDate = DateTime.Now.Date,
                    TotalMembers = memberCount
                }).ConfigureAwait(false);
                await Context.SaveChangesAsync().ConfigureAwait(false);
                return server.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
