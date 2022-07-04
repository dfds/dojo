using Demo.Domain.Entities;
using System.Linq.Expressions;

namespace Demo.Domain.Repositories
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Get a specific Category(model) by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Category> GetById(int id);

        /// <summary>
        /// Get entities depending on the filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<Category>> GetAll(Expression<Func<Category, bool>>? filter = null);
        /// <summary>
        /// Insert new Category
        /// </summary>
        /// <param name="Category"></param>
        void Insert(Category category);
        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="category"></param>
        void Delete(Category category);
        /// <summary>
        /// Update a category
        /// </summary>
        /// <param name="category"></param>
        void Update(Category category);

    }
}
