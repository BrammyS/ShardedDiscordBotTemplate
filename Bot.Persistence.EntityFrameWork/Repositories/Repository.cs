using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bot.Persistence.Repositories;

namespace Bot.Persistence.EntityFrameWork.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly BotContext Context;

        public Repository(BotContext context)
        {
            Context = context;
        }

        public async Task AddAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity).ConfigureAwait(false);
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await Context.Set<T>().AddRangeAsync(entities).ConfigureAwait(false);
        }

        public IEnumerable<T> Where(Expression<System.Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }

        public T Get(ulong id)
        {
            return Context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }

        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
        }
    }
}

