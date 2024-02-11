namespace CIS.Core.Domain;

public interface IAggregateRoot
{
    void Execute(ICommand<IAggregateRoot> command);
    TResult Execute<TResult>(ICommand<IAggregateRoot, TResult> command);    
}

public interface ICommand<TTarget> where TTarget : IAggregateRoot
{
    void Apply(TTarget target);
}

public interface ICommand<TTarget, TResult> where TTarget : IAggregateRoot
{
    TResult Apply(IAggregateRoot target);
}
