using MassTransit;
using Microsoft.EntityFrameworkCore;
using TransactionalSystem.Messaging;
using Transactions.Api.Consumers;
using Transactions.Api.Infrastructure;
using Transactions.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMessagingInfrastructure(builder.Configuration, typeof(AccountsRemovedConsumer).Assembly, config =>
{
    config.AddActivities(typeof(CreateTransactionActivity).Assembly);
});
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ITransactionsDbContext, TransactionsDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString(ITransactionsDbContext.ConnectionStringName)));
var app = builder.Build();
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ITransactionsDbContext>() as TransactionsDbContext;

dbContext?.Database.Migrate();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/balances", async (Guid[] customerIds, ITransactionsDbContext transactionContext) =>
{
    var result = await transactionContext.Transactions
        .Where(x => customerIds.Contains(x.CustomerId))
        .GroupBy(t => t.CustomerId)
        .Select(
            group => new BalanceResponse
            {
                CustomerId = group.Key,
                Balance = group.Sum(t => t.Value),
                Transactions = group.Select(t => new TransactionsResponse(t.Id, t.AccountId, t.Value)).ToArray()
            }).ToListAsync();
    return Results.Ok(result);
});
app.Run();