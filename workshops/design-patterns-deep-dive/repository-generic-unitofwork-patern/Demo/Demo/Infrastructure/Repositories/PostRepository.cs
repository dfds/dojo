using Demo.Domain.Entities;
using Demo.Domain.Repositories;
using Demo.Infrastructure.Context;
using Demo.Infrastructure.Repositories;

namespace Demo.Infrastructure.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(DemoDbContext dbContext) : base(dbContext)
        {

        }
    }

}
