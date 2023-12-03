namespace Customers.Api.Domain;

public sealed class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    public Customer()
    {
    }

    private Customer(string name, string lastName, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        LastName = lastName;
        Email = email;
    }

    public static Customer Create(string name, string lastName, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        
        return new Customer(name, lastName, email);
    }
    
}