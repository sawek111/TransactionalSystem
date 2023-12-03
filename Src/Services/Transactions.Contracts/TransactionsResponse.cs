namespace Transactions.Contracts;

public record TransactionsResponse(Guid TransactionId, Guid AccountId, decimal Amount); 