namespace Health.Patient.Domain.Core.Mediator.Queries;

public interface IAsyncQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery command);
}