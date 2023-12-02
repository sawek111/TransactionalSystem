using System.ComponentModel.DataAnnotations;

namespace TransactionalSystem.Messaging;
// TODO extract whole dir to sep project
public class EventBusSettings
{
    public const string SectionName = "EventBusSettings";
    
    [Required]
    public string Host { get; init; }
    
    [Required]
    public string UserName { get; init; }
    
    [Required]
    public string Password { get; init; }

}
