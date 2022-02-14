using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Queries.Core;

namespace Health.Patient.Domain.Core.Mediator;

public interface IMediator
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command);
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query);
}