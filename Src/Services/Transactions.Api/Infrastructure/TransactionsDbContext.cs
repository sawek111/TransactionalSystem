using Microsoft.EntityFrameworkCore;
using Transactions.Api.Domain;

namespace Transactions.Api.Infrastructure;

public class TransactionsDbContext : DbContext, ITransactionsDbContext
{
    public DbSet<Transaction> Transactions { get; private set; }
    public DbSet<Account> Accounts { get; private set; }

    public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options)
        : base(options)
    {
    }
    
    // TODO to fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.HasOne<Transaction>()
                .WithOne()
                .HasForeignKey<Transaction>(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
        });
    }
}