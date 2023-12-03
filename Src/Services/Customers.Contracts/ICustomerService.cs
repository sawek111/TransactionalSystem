namespace Customers.Contracts;

public interface ICustomerService
{
    Task<IEnumerable<CustomerResponse>> GetCustomers();
}