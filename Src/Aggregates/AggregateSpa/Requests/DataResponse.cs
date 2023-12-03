using Transactions.Contracts;

namespace AggregateSpa.Requests;

public record DataResponse(Guid CustomerId, string Name, string Surname, decimal Balance, IEnumerable<TransactionsResponse> Transactions);
