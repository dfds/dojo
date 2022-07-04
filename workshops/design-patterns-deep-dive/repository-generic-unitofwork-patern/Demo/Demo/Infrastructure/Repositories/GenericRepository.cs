using Demo.Domain.Common;
using Demo.Domain.Repositories;
using Demo.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Demo.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity>where  TEntity: BaseEntity
    {
        /// <summary>
        /// It's protected because I wanted to be able to access this field from the children(PostRepository,CategoryRepository, TagRepository...) of this baseRepository.
        /// </summary>
        protected readonly DbSet<TEntity> DbSet;
        public GenericRepository(DemoDbContext dbContext) => DbSet = dbContext.Set<TEntity>();
        public void Delete(TEntity entity) => DbSet.Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? filter) => 
            filter != null ? await DbSet.Where(filter).ToListAsync() : await DbSet.ToListAsync();

        public async Task<TEntity> GetById(int id) => await DbSet.FindAsync(id);

        public void Insert(TEntity entity) => DbSet.Add(entity);

        public void Update(TEntity entity) => DbSet.Update(entity);
    }
}
