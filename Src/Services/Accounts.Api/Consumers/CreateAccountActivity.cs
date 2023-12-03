using Accounts.Api.Domain;
using Accounts.Api.Infrastructure;
using Accounts.Contracts;
using MassTransit;

namespace Accounts.Api.Consumers;

public class CreateAccountActivity(IAccountsDbContext accountsDbContext) : IActivity<AccountCreationRequested, AccountCreationFailed>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<AccountCreationRequested> context)
    {
        var account = Account.Create(context.Arguments.CustomerId);
        accountsDbContext.Accounts.Add(account);
        await accountsDbContext.SaveChangesAsync();
        
        return context.Arguments.InitialCredit <= 0 
            ? context.Terminate() 
            : context.Completed<AccountCreationFailed>(new AccountCreationFailed(AccountId: account.Id));
    }

    public async Task<CompensationResult> Compensate(CompensateContext<AccountCreationFailed> context)
    {
        var account = await accountsDbContext.Accounts.FindAsync(context.Log.AccountId);
        if (account is not null)
        {
            accountsDbContext.Accounts.Remove(account);
        }

        await accountsDbContext.SaveChangesAsync();

        return context.Compensated();
    }
}