using Demo.Domain.Repositories;
using Demo.Infrastructure.Context;

namespace Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        public IPostRepository Posts { get; private set; }
        public ICategoryRepository Categories { get; private set; }

        private readonly DemoDbContext _dbContext;

        public UnitOfWork(IPostRepository posts, ICategoryRepository categories, DemoDbContext dbContext)
        {
            Posts = posts;
            Categories = categories;
            _dbContext = dbContext;
        }
        public int Save() => _dbContext.SaveChanges();
    }
}
