namespace TransactionalSystem.Messaging;

public interface IIntegrationEvent
{
    public Guid EventId { get; }
    public DateTime CreatedAtUtc { get; }
}