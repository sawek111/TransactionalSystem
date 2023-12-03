namespace Accounts.Api.Domain;

public sealed class Account
{
    public static Account Create(Guid customerId, Guid? id = null)
    {
        return new Account()
        {
            Id = id ?? Guid.NewGuid(),
            CustomerId = customerId
        };
    }

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
}