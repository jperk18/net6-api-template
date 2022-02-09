namespace Health.Patient.Domain.Queries.Core;

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery command);
}