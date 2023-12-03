using Customers.Contracts;

namespace AggregateSpa;

public sealed class CustomerService(HttpClient httpClient) : ICustomerService
{
    public async Task<IEnumerable<CustomerResponse>> GetCustomers()
    {
        const string urlPostfix = "customers";
        try
        {
            var response = await httpClient.GetFromJsonAsync<IEnumerable<CustomerResponse>>(urlPostfix);
            return response ?? Enumerable.Empty<CustomerResponse>();
        }
        catch (Exception e)
        {
            return Enumerable.Empty<CustomerResponse>();
        }
    }
}