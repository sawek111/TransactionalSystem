using Customers.Contracts;
using Transactions.Contracts;

namespace AggregateSpa;

public sealed class TransactionsService(HttpClient httpClient) : ITransactionsService
{
    public async Task<IEnumerable<BalanceResponse>> GetBalancesWithHistory(List<Guid> customerIds)
    {
        const string urlPostfix = "balances";
        try
        {
            var queryParams = new List<KeyValuePair<string, string>>();
            foreach (var id in customerIds)
            {
                queryParams.Add(new KeyValuePair<string, string>("customerIds", id.ToString()));
            }
            var query = new FormUrlEncodedContent(queryParams).ReadAsStringAsync().Result;
            var urlWithQuery = $"{urlPostfix}?{query}";
            var response = await httpClient.GetFromJsonAsync<List<BalanceResponse>>(urlWithQuery);
            return response ?? Enumerable.Empty<BalanceResponse>();
        }
        catch (Exception e)
        {
            return Enumerable.Empty<BalanceResponse>();
        }
    }
}