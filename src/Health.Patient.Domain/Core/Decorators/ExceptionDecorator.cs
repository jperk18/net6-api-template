using FluentValidation;
using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Core.Exceptions;
using Health.Patient.Domain.Queries.Core;
using Microsoft.Extensions.Logging;

namespace Health.Patient.Domain.Core.Decorators;

public sealed class ExceptionCommandDecorator<TCommand, TOutput> : ICommandHandler<TCommand, TOutput>
    where TCommand : ICommand<TOutput>
{
    private readonly ILogger<ExceptionCommandDecorator<TCommand, TOutput>> _logger;
    private readonly ICommandHandler<TCommand, TOutput> _handler;

    public ExceptionCommandDecorator(ILogger<ExceptionCommandDecorator<TCommand, TOutput>> logger,ICommandHandler<TCommand, TOutput> handler)
    {
        _logger = logger;
        _handler = handler;
    }

    public async Task<TOutput> Handle(TCommand command)
    {
        try
        {
            return await _handler.Handle(command);
        }
        catch (ValidationException e)
        {
            throw new DomainValidationException(e);
        }
    }
}

public sealed class ExceptionQueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    private readonly ILogger<ExceptionQueryDecorator<TQuery, TResult>> _logger;
    private readonly IQueryHandler<TQuery, TResult> _handler;

    public ExceptionQueryDecorator(ILogger<ExceptionQueryDecorator<TQuery, TResult>> logger, IQueryHandler<TQuery, TResult> handler)
    {
        _logger = logger;
        _handler = handler;
    }

    public async Task<TResult> Handle(TQuery query)
    {
        try
        {
            return await _handler.Handle(query);
        }
        catch (ValidationException e)
        {
            throw new DomainValidationException(e);
        }
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class ExceptionPipelineAttribute : Attribute
{
    public ExceptionPipelineAttribute()
    {
    }
}