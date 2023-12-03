using Accounts.Api.Domain;
using Accounts.Api.Infrastructure;
using Accounts.Contracts;
using Customers.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Api.Consumers;

public sealed class AllCustomersDeletedConsumer(IAccountsDbContext accountsDbContext, IBus bus) : IConsumer<AllCustomersDeletedEvent>
{
    public async Task Consume(ConsumeContext<AllCustomersDeletedEvent> context)
    {
        accountsDbContext.Customers.RemoveRange(accountsDbContext.Customers);
        var accountsIds = await accountsDbContext.Accounts.Select(x => x.Id).ToListAsync();
        await accountsDbContext.SaveChangesAsync();
        await bus.Publish(new AccountsDeletedEvent(accountsIds));
    }
}