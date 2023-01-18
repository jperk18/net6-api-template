using Health.Patient.Domain.Core.Mediator.Commands;
using Health.Patient.Domain.Core.Mediator.Queries;

namespace Health.Patient.Domain.Core.Mediator;

public interface IMediator
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command);
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query);
}