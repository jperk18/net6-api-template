namespace Health.Patient.Domain.Core.Mediator.Commands;

public interface IAsyncCommandHandler<TCommand, TOutput> where TCommand : ICommand<TOutput>
{
    Task<TOutput> Handle(TCommand command);
}