using Microsoft.EntityFrameworkCore;
using TransactionalSystem.Messaging;
using Transactions.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMessagingInfrastructure(builder.Configuration, typeof(Program).Assembly);
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
app.Run();