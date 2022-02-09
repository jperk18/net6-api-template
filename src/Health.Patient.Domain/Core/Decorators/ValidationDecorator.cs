using FluentValidation;
using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Queries.Core;
using Microsoft.Extensions.Logging;

namespace Health.Patient.Domain.Core.Decorators;

public sealed class ValidationCommandDecorator<TCommand, TOutput> : ICommandHandler<TCommand, TOutput>
    where TCommand : ICommand<TOutput>
{
    private readonly ILogger<ValidationCommandDecorator<TCommand, TOutput>> _logger;
    private readonly IEnumerable<IValidator<TCommand>> _validators;
    private readonly ICommandHandler<TCommand, TOutput> _handler;

    public ValidationCommandDecorator(ILogger<ValidationCommandDecorator<TCommand, TOutput>> logger,
        IEnumerable<IValidator<TCommand>> validators, ICommandHandler<TCommand, TOutput> handler)
    {
        _logger = logger;
        _validators = validators;
        _handler = handler;
    }

    public async Task<TOutput> Handle(TCommand command)
    {
        if (!_validators.Any())
        {
            _logger.LogInformation($"No validation for {command.GetType().Name}");

            return await _handler.Handle(command);
        }

        _logger.LogInformation($"Running validation for {command.GetType().Name}");
        var context = new ValidationContext<TCommand>(command);
        var errors = _validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToArray();

        if (errors.Any())
        {
            _logger.LogInformation($"Validation error occured for {command.GetType().Name}");
            throw new ValidationException(errors);
        }

        _logger.LogInformation($"Validation for {command.GetType().Name} successful");
        return await _handler.Handle(command);
    }
}

public sealed class ValidationQueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    private readonly ILogger<ValidationQueryDecorator<TQuery, TResult>> _logger;
    private readonly IEnumerable<IValidator<TQuery>> _validators;
    private readonly IQueryHandler<TQuery, TResult> _handler;

    public ValidationQueryDecorator(ILogger<ValidationQueryDecorator<TQuery, TResult>> logger,
        IEnumerable<IValidator<TQuery>> validators, IQueryHandler<TQuery, TResult> handler)
    {
        _logger = logger;
        _validators = validators;
        _handler = handler;
    }

    public async Task<TResult> Handle(TQuery query)
    {
        if (!_validators.Any())
        {
            _logger.LogInformation($"No validation for {query.GetType().Name}");
            return await _handler.Handle(query);
        }

        _logger.LogInformation($"Running validation for {query.GetType().Name}");
        var context = new ValidationContext<TQuery>(query);
        var errors = _validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToArray();

        if (errors.Any())
        {
            _logger.LogInformation($"Validation error occured for {query.GetType().Name}");
            throw new ValidationException(errors);
        }

        _logger.LogInformation($"Validation for {query.GetType().Name} successful");
        return await _handler.Handle(query);
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class ValidationPipelineAttribute : Attribute
{
    public ValidationPipelineAttribute()
    {
    }
}