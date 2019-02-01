using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bot.Persistence.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Get(ulong id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);

        void Add(T entity);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}

