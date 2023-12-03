using Accounts.Contracts;
using Accounts.Contracts.Requests;
using MassTransit;
using TransactionalSystem.Messaging;
using Transactions.Contracts;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMessagingInfrastructure(builder.Configuration, typeof(Program).Assembly);

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
                request.InitialCredit));

        var routingSlip = routingSlipBuilder.Build();
        await bus.Execute(routingSlip);
        return Results.Ok();
    });
app.Run();