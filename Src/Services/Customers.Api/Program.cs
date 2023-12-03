using Customers.Api.Infrastructure;
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
    "customers", async (ICustomersDbContext dbContext, ICustomerGenerator generator) =>
    {
        if (!dbContext.Customers.Any())
        {
           await generator.Generate(ICustomersDbContext.InitCount);
        }
        
        return Results.Ok(await dbContext.Customers.ToListAsync());
    });

app.Run();