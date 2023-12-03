namespace Accounts.Api.Domain;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    private Customer()
    {
    }
    
    public static Customer Create(Guid id, string name)
    {
        return new Customer()
        {
            Id = id,
            Name = name,
        };
    }
}