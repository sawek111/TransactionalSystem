namespace Transactions.Contracts;

public record TransactionCreationRequested(Guid AccountId, decimal Value)
{
    public const string TopicName = "create-transaction_execute";
}