using MassTransit;
using Transactions.Api.Domain;
using Transactions.Api.Infrastructure;
using Transactions.Contracts;

namespace Transactions.Api.Consumers;

public class CreateTransactionActivity(ITransactionsDbContext transactionsDbContext) : IActivity<TransactionCreationRequested, TransactionCreationFailed>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<TransactionCreationRequested> context)
    {
        var transaction = Transaction.Create(context.Arguments.AccountId, context.Arguments.value);
        transactionsDbContext.Transactions.Add(transaction);
        await transactionsDbContext.SaveChangesAsync();
        
        return context.Completed(new TransactionCreationFailed(TransactionId: transaction.Id));
    }

    public async Task<CompensationResult> Compensate(CompensateContext<TransactionCreationFailed> context)
    {
        var transaction = await transactionsDbContext.Transactions.FindAsync(context.Log.TransactionId);
        if (transaction is not null)
        {
            transactionsDbContext.Transactions.Remove(transaction);
        }

        return context.Compensated();
    }
}