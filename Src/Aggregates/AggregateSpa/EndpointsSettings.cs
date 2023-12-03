using System.ComponentModel.DataAnnotations;

namespace AggregateSpa;

public class EndpointsSettings
{
    public const string SectionName = "EndpointsSettings";
    
    [Required]
    public string CustomersHost { get; init; }
    
    [Required]
    public string TransactionsHost { get; init; }
}