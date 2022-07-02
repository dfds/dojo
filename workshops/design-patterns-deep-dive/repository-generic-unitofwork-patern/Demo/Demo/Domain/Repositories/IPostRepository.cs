using Demo.Domain.Entities;
using System.Linq.Expressions;

namespace Demo.Domain.Repositories
{
    public interface IPostRepository
    {
        /// <summary>
        /// Get a specific post(model) by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Post> GetById(int id);

        /// <summary>
        /// Get entities depending on the filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<Post>> GetAll(Expression<Func<Post, bool>>? filter = null);
        /// <summary>
        /// Insert new post
        /// </summary>
        /// <param name="post"></param>
        void Insert(Post post);
        /// <summary>
        /// Delete an post
        /// </summary>
        /// <param name="post"></param>
        void Delete(Post post);
        /// <summary>
        /// Update an post
        /// </summary>
        /// <param name="post"></param>
        void Update(Post post);

    }
}
