using Demo.Domain.Entities;
using Demo.Domain.Repositories;
using Demo.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Demo.Infrastructure.Repositories
{
    public class CategoryRepository: GenericRepository<Category>, ICategoryRepository
    {
        /// <summary>
        /// If you want to do some data access operations which is specific to category repository, 
        /// you can add a new method as below and continue without changing the generic class.
        /// </summary>
        /// <param name="dbContext"></param>
        public CategoryRepository(DemoDbContext dbContext) : base(dbContext)
        {

        }
       
        public async Task<IEnumerable<Category>> GetCategoryWithPosts(Expression<Func<Category, bool>>? filter)
        {
            return filter != null ? await DbSet.Where(filter).Include(category=>category.Posts).ToListAsync() : await DbSet.Include(category => category.Posts).ToListAsync();
        }
    }
}
