using Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Context
{
    public class DemoDbContext:DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options):base(options)
        {
           
        }
        public DbSet<Post> Posts { get; set; }    
        public DbSet<Category> Categoriess { get; set; }
    }
}
