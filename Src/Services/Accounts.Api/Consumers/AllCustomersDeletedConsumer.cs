using Accounts.Api.Domain;
using Accounts.Api.Infrastructure;
using Customers.Contracts;
using MassTransit;

namespace Accounts.Api.Consumers;

public sealed class AllCustomersDeletedConsumer(IAccountsDbContext accountsDbContext) : IConsumer<CustomerCreatedEvent>
{
    public async Task Consume(ConsumeContext<CustomerCreatedEvent> context)
    {
        var customer = Customer.Create(context.Message.Id, context.Message.Name);
        accountsDbContext.Customers.Add(customer);
        await accountsDbContext.SaveChangesAsync();
    }
}