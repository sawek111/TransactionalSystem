using Accounts.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Api.Infrastructure;

public interface IAccountsDbContext
{
    public const string ConnectionStringName = "AccountsDb";
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public DbSet<Customer> Customers { get; }
    public DbSet<Account> Accounts { get; }

}