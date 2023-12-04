namespace Customers.Contracts;

public interface ICustomerService
{
    Task<IEnumerable<CustomerResponse>> GetCustomers();
    Task<CustomerResponse?> GetCustomer(Guid customerId);
}