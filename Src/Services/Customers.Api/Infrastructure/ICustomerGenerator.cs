namespace Customers.Api.Infrastructure;

public interface ICustomerGenerator
{
    Task Generate(int count);
}