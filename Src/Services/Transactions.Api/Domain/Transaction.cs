namespace Transactions.Api.Domain;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public decimal Value { get; private set; }
    public static Transaction Create(Guid accountId, decimal value, Guid? id = null)
    {
        return new Transaction()
        {
            Id = id ?? Guid.NewGuid(),
            AccountId = accountId,
            Value = value
        };
    }
}