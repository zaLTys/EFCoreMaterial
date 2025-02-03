namespace DataAccessLayer.UoW;

public interface IUnitOfWork : IDisposable
{
    MyDbContext Context { get; }

    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();

    Task<int> SaveChangesAsync();
}
