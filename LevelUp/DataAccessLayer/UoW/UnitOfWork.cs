using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccessLayer.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly MyDbContext _context;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(MyDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public MyDbContext Context => _context;

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No active transaction to commit.");
        }

        try
        {
            await _currentTransaction.CommitAsync();
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No active transaction to roll back.");
        }

        try
        {
            await _currentTransaction.RollbackAsync();
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        // Leverage EF Core’s default save behavior
        // Could include logic for auditing, timestamps, etc.
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _context?.Dispose();
    }
}
