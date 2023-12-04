using Accounts.Api;
using Accounts.Api.Consumers;
using Accounts.Api.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TransactionalSystem.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMessagingInfrastructure(builder.Configuration, typeof(CustomerCreatedConsumer).Assembly, config =>
{
    config.AddActivities(typeof(CreateAccountActivity).Assembly);
});
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<IAccountsDbContext, AccountsDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString(IAccountsDbContext.ConnectionStringName)));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<IAccountsDbContext>() as AccountsDbContext;
var pendingMigrations = dbContext!.Database.GetPendingMigrations();
if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();