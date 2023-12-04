using Customers.Api.Infrastructure;
using Customers.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TransactionalSystem.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMessagingInfrastructure(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomerGenerator, CustomerGenerator>();
builder.Services.AddDbContext<ICustomersDbContext, CustomersDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString(ICustomersDbContext.ConnectionStringName)));
var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ICustomersDbContext>() as CustomersDbContext;
dbContext?.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet(
    "customers", async (ICustomersDbContext customersDbContext, ICustomerGenerator generator) =>
    {
        if (!customersDbContext.Customers.Any())
        {
           await generator.Generate(ICustomersDbContext.InitCount);
        }
        
        return Results.Ok(await customersDbContext.Customers.ToListAsync());
    });

app.MapGet(
    "customers/{customerId}", async (Guid customerId, ICustomersDbContext customersDbContext) =>
    {
        var customer = await customersDbContext.Customers.FindAsync(customerId);
        return customer is not null ? Results.Ok(customer) : Results.NotFound(customerId);
    });

app.MapDelete(
    "customers", async (ICustomersDbContext customersDbContext, IBus bus) =>
    {
        customersDbContext.Customers.RemoveRange(customersDbContext.Customers);
        var removedAmount = await customersDbContext.SaveChangesAsync();
        await bus.Publish(new AllCustomersDeletedEvent());
        return Results.Ok(removedAmount);
    });

app.Run();