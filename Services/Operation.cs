namespace HRIS.API.Services;

public interface IOperation
{
    Guid OperaionId { get; set; }
}

public interface ITransactionOperation :  IOperation{}
public interface IScopeOperation : IOperation{}
public interface ISingletonOperation : IOperation{}

public class Operation : ITransactionOperation, IScopeOperation, ISingletonOperation
{
    public Guid OperaionId { get; set; } =  Guid.NewGuid();
}