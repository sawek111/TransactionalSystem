namespace AggregateSpa.Requests;

public sealed record CreateAccountRequest(Guid CustomerId, decimal InitialCredit);