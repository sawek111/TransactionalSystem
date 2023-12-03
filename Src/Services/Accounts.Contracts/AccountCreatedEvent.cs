using TransactionalSystem.Messaging;

namespace Accounts.Contracts;

public sealed record AccountCreatedEvent(Guid CorrelationId, Guid AccountId) : IntegrationBaseEvent
{
}