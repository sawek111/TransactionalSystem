using TransactionalSystem.Messaging;

namespace Customers.Contracts;

public sealed record CustomerCreatedEvent(string Name, Guid Id) : IntegrationBaseEvent;