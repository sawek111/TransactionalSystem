namespace Accounts.Contracts;

public sealed record AccountCreationRequested(Guid NewAccountId, Guid CustomerId, decimal InitialCredit)
{
    public const string TopicName = "accounts-created-queue"; 
}