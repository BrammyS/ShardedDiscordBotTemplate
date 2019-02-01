using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bot.Persistence.Repositories
{
    public interface IRepository<T> where T : class
    {


        /// <summary>
        /// Returns the requested <see cref="T"/> from the database with an Id.
        /// </summary>
        /// <param name="id">The id of the object.</param>
        /// <returns><see cref="T"/></returns>
        T Get(ulong id);


        /// <summary>
        /// Gets all the objects from the table.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        IEnumerable<T> GetAll();


        /// <summary>
        /// Gets all the objects where the expression is true.
        /// </summary>
        /// <param name="predicate">The expression that will be used to find the objects.</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);


        /// <summary>
        /// Adds an object to the database.
        /// </summary>
        /// <param name="entity">Entity that will be added to the database.</param>
        void Add(T entity);


        /// <summary>
        /// Adds an object to the database asynchronously. 
        /// </summary>
        /// <param name="entity">Entity that will be added to the database.</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        Task AddAsync(T entity);


        /// <summary>
        /// Adds a <see cref="List{T}"/> of objects to the database asynchronously. 
        /// </summary>
        /// <param name="entities">Entities that will be added to the database.</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        Task AddRangeAsync(IEnumerable<T> entities);


        /// <summary>
        /// Removes an object from the database.
        /// </summary>
        /// <param name="entity">The entity that will be removed from the database.</param>
        void Remove(T entity);


        /// <summary>
        /// Remove a <see cref="List{T}"/> of objects from the database.
        /// </summary>
        /// <param name="entities">The entities that will be removed from the database.</param>
        void RemoveRange(IEnumerable<T> entities);
    }
}

