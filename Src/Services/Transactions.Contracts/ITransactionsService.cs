namespace Transactions.Contracts;

public interface ITransactionsService
{
    public Task<IEnumerable<BalanceResponse>> GetBalancesWithHistory(List<Guid> customerIds);
}