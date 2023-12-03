using Accounts.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Transactions.Api.Domain;
using Transactions.Api.Infrastructure;

namespace Transactions.Api.Consumers;

public sealed class AccountsRemovedConsumer(ITransactionsDbContext transactionsDbContext) : IConsumer<AccountsDeletedEvent>
{
    public async Task Consume(ConsumeContext<AccountsDeletedEvent> context)
    {
        RemoveBatch(context, transactionsDbContext);
        await transactionsDbContext.SaveChangesAsync();
    }

    private static void RemoveBatch(ConsumeContext<AccountsDeletedEvent> context, ITransactionsDbContext transactionsDbContext)
    {
        foreach (var accountToDelete in context.Message.DeletedIds.Select(id => new Account { Id = id }))
        {
            transactionsDbContext.Entry(accountToDelete).State = EntityState.Deleted;
        }
    }
}