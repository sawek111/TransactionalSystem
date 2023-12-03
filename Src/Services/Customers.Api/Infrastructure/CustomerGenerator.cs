using Bogus;
using Customers.Api.Domain;
using Customers.Contracts;
using MassTransit;

namespace Customers.Api.Infrastructure;

public sealed class CustomerGenerator(ICustomersDbContext customersContext, IBus bus) : ICustomerGenerator
{
    // TODO replace with user ingterface
    public async Task Generate(int count)
    {
        var customers = GenerateCustomers(count);
        var result = await customersContext.SaveChangesAsync();
        if (result > 0)
        {
            foreach (var customer in customers)
            {
                await bus.Publish(new CustomerCreatedEvent(customer.Name, customer.Id));
            }
        }
    }

    private IEnumerable<Customer> GenerateCustomers(int count)
    {
        var userGenerator = new Faker<Customer>()
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Name, f => f.Name.FirstName());
        var fakedCustomers = userGenerator.Generate(count);
        var customers = new Customer[count];
        for (var i = 0; i < count; i++)
        {
            var faked = fakedCustomers[i];
            var customer = Customer.Create(faked.Name, faked.LastName, faked.Email);
            customers[i] = customer;
        }
        customersContext.Customers.AddRange(customers);
        return customers;
    }
}