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

        /// <summary>
        /// Creates a new <see cref="Repository{T}"/>.
        /// </summary>
        /// <param name="context">The context that will be used.</param>
        public Repository(BotContext context)
        {
            Context = context;
        }


        /// <inheritdoc/>
        public async Task AddAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity).ConfigureAwait(false);
        }



        /// <inheritdoc/>
        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }


        /// <inheritdoc/>
        public Task AddRangeAsync(IEnumerable<T> entities)
        {
            return Context.Set<T>().AddRangeAsync(entities);
        }


        /// <inheritdoc/>
        public IEnumerable<T> Where(Expression<System.Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }


        /// <inheritdoc/>
        public T Get(ulong id)
        {
            return Context.Set<T>().Find(id);
        }


        /// <inheritdoc/>
        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }


        /// <inheritdoc/>
        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
        }


        /// <inheritdoc/>
        public void RemoveRange(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
        }
    }
}

