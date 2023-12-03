using TransactionalSystem.Messaging;

namespace Customers.Contracts;

public sealed record AllCustomersDeletedEvent() : IntegrationBaseEvent;