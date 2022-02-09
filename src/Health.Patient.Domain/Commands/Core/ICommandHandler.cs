namespace Health.Patient.Domain.Commands.Core;

public interface ICommandHandler<TCommand, TOutput> where TCommand : ICommand<TOutput>
{
    Task<TOutput> Handle(TCommand command);
}