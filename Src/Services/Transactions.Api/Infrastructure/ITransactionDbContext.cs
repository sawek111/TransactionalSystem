using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Transactions.Api.Domain;

namespace Transactions.Api.Infrastructure;

public interface ITransactionsDbContext
{
    public const string ConnectionStringName = "TransactionsDb";
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public DbSet<Transaction> Transactions { get; }
    public DbSet<Account> Accounts { get; }

}