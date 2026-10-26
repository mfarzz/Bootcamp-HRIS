namespace HRIS.API.Services;

public class OperationReport(
    ITransactionOperation transaction,
    IScopeOperation scope,
    ISingletonOperation singleton
    )
{
    public ITransactionOperation Transaction { get; } = transaction;
    public IScopeOperation Scope { get; } = scope;
    public ISingletonOperation Singleton { get; } = singleton;
}