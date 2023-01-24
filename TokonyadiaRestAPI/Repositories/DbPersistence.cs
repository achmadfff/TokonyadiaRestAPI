using Microsoft.EntityFrameworkCore;

namespace TokonyadiaRestAPII.Repositories;

public class DbPersistence : IPersistence
{
    private readonly AppDBContext _context;

    public DbPersistence(AppDBContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<TResult> ExecuteTransactionAsync<TResult>(Func<Task<TResult>> func)
    {   
        var strategy = _context.Database.CreateExecutionStrategy();
        var transResult = await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = await func();
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        });

        return transResult;
    }
}