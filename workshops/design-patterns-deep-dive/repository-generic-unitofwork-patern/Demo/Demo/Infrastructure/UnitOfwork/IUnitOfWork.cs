using Demo.Domain.Repositories;

namespace Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IPostRepository Posts { get; }
        ICategoryRepository Categories { get; }
        int Save();
    }
}
