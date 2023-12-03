using TransactionalSystem.Messaging;

namespace Accounts.Contracts;

public sealed record AccountsDeletedEvent(List<Guid> DeletedIds) : IntegrationBaseEvent;