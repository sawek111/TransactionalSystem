using Accounts.Api;
using Accounts.Api.Consumers;
using Accounts.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TransactionalSystem.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMessagingInfrastructure(builder.Configuration, typeof(CustomerCreatedConsumer).Assembly);
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<IAccountsDbContext, AccountsDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString(IAccountsDbContext.ConnectionStringName)));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<IAccountsDbContext>() as AccountsDbContext;
dbContext?.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();