using Customers.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Customers.Api.Infrastructure;

public interface ICustomersDbContext
{
    public const string ConnectionStringName = "CustomersDb";
    public const int InitCount = 20;
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public DbSet<Customer> Customers { get; }
}