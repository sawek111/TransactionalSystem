namespace Accounts.Contracts.Requests;

public sealed record CreateAccountRequest(Guid CustomerId, decimal InitialCredit);