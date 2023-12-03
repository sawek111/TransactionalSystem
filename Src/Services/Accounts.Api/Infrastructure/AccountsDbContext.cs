using Accounts.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Api.Infrastructure;

public class AccountsDbContext : DbContext, IAccountsDbContext
{
    public DbSet<Customer> Customers { get; private set; }
    public DbSet<Account> Accounts { get; private set; }

    public AccountsDbContext(DbContextOptions<AccountsDbContext> options)
        : base(options)
    {
    }
    
    // TODO to fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
        });
        
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
}