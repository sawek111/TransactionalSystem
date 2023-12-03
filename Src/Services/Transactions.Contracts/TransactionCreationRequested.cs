namespace Transactions.Contracts;

public record TransactionCreationRequested(Guid AccountId, Guid CustomerId, decimal Value)
{
    public const string TopicName = "create-transaction_execute";
}