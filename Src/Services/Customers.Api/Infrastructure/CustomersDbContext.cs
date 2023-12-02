using Customers.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Customers.Api.Infrastructure;

public class CustomersDbContext : DbContext, ICustomersDbContext
{
    public DbSet<Customer> Customers { get; private set; }

    public CustomersDbContext(DbContextOptions<CustomersDbContext> options)
        : base(options)
    {
        Database.Migrate();
    }
    
    // TODO to fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Email).IsRequired();
        });
    }
    
}