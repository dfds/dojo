using Demo.Domain.Common;
using System.Linq.Expressions;

namespace Demo.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// Get a specific entity(model) by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetById(int id);

        /// <summary>
        /// Get entities dep
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? filter = null);
        /// <summary>
        /// Insert new entity
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity); 
        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

    }
}
