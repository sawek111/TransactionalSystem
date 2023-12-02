using MassTransit;

namespace TransactionalSystem.Messaging;

public interface IEventConsumer<in TMessage> : IConsumer<TMessage> where TMessage : IntegrationBaseEvent
{
}