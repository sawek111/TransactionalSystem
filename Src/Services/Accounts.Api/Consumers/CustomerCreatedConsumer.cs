﻿using Accounts.Api.Domain;
using Accounts.Api.Infrastructure;
using Accounts.Contracts;
using Customers.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Api.Consumers;

public sealed class CustomerCreatedConsumer(IAccountsDbContext accountsDbContext, IBus bus) : IConsumer<CustomerCreatedEvent>
{
    public async Task Consume(ConsumeContext<CustomerCreatedEvent> context)
    {
        var customer = Customer.Create(context.Message.Id, context.Message.Name);
        accountsDbContext.Customers.Add(customer);
        await accountsDbContext.SaveChangesAsync();
    }
}