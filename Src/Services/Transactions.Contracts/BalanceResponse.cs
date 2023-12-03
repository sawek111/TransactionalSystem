namespace Transactions.Contracts;

public record BalanceResponse()
{
    public static BalanceResponse Create(Guid customerId, decimal balance, IEnumerable<TransactionsResponse> transactions)
    {
        return new BalanceResponse
        {
            CustomerId = customerId,
            Balance = balance,
            Transactions = transactions.ToArray()
        };
    }

    public Guid CustomerId { get; init; }
    public decimal Balance { get; init; }
    public TransactionsResponse[] Transactions { get; init; } = Array.Empty<TransactionsResponse>();
}