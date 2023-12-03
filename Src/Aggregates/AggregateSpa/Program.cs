using Accounts.Contracts;
using AggregateSpa;
using AggregateSpa.Requests;
using Customers.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TransactionalSystem.Messaging;
using Transactions.Contracts;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMessagingInfrastructure(builder.Configuration, typeof(Program).Assembly);

builder.Services.AddOptions<EndpointsSettings>()
    .BindConfiguration(EndpointsSettings.SectionName)
    .ValidateDataAnnotations();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<EndpointsSettings>>().Value);
builder.Services.AddHttpClient<ICustomerService, CustomerService>(
    (serviceProvider, client) => { client.BaseAddress = new Uri(serviceProvider.GetRequiredService<EndpointsSettings>().CustomersHost); });
builder.Services.AddHttpClient<ITransactionsService, TransactionsService>(
    (serviceProvider, client) => { client.BaseAddress = new Uri(serviceProvider.GetRequiredService<EndpointsSettings>().TransactionsHost); });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.RoutePrefix = string.Empty;
        });
}

app.UseHttpsRedirection();

app.MapGet(
    "/data", async ([FromServices] ICustomerService customerService, ITransactionsService transactionsService) =>
    {
        // TODO: TO CQRS
        var customers = (await customerService.GetCustomers()).ToList();
        var balances = await transactionsService.GetBalancesWithHistory(customers.Select(x => x.Id).ToList());

        var balancesDictionary = balances.ToDictionary(b => b.CustomerId);
        var combinedList = new List<DataResponse>();
        foreach (var customer in customers)
        {
            var balance = balancesDictionary.TryGetValue(customer.Id, out var value) ? value : BalanceResponse.Create(customer.Id, 0.0m, Enumerable.Empty<TransactionsResponse>());
            var combinedInfo = new DataResponse(customer.Id, customer.Name, customer.LastName, balance.Balance, balance.Transactions);
            combinedList.Add(combinedInfo);
        }

        return Results.Ok(combinedList);
    });

app.MapPost(
    "/account", async (CreateAccountRequest request, IBus bus, EventBusSettings settings) =>
    {
        // TODO: TO CQRS
        var newAccountId = Guid.NewGuid();
        var rabbitMqUri = new Uri($"rabbitmq://{settings.Host}");

        var accountCreationRequestedUri = new Uri(rabbitMqUri, AccountCreationRequested.TopicName);
        var transactionCreationRequestedUri = new Uri(rabbitMqUri, TransactionCreationRequested.TopicName);

        var routingSlipBuilder = new RoutingSlipBuilder(NewId.NextGuid());
        routingSlipBuilder.AddActivity(
            AccountCreationRequested.TopicName, accountCreationRequestedUri, new AccountCreationRequested(newAccountId, request.CustomerId, request.InitialCredit));
        routingSlipBuilder.AddActivity(
            TransactionCreationRequested.TopicName, transactionCreationRequestedUri, new TransactionCreationRequested(
                newAccountId,
                request.CustomerId,
                request.InitialCredit));

        var routingSlip = routingSlipBuilder.Build();
        await bus.Execute(routingSlip);
        return Results.Ok();
    });

app.Run();