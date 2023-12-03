namespace Transactions.Contracts;

public record TransactionCreationRequested(Guid AccountId, decimal value)
{
    public const string TopicName = "transactions-created-queue";
}