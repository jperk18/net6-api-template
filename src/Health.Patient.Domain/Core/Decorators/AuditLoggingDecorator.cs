using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Core.Exceptions;
using Health.Patient.Domain.Queries.Core;
using Microsoft.Extensions.Logging;

namespace Health.Patient.Domain.Core.Decorators;

public sealed class AuditLoggingCommandDecorator<TCommand, TOutput> : ICommandHandler<TCommand, TOutput>
    where TCommand : ICommand<TOutput>
{
    private readonly ILogger<AuditLoggingCommandDecorator<TCommand, TOutput>> _logger;
    private readonly ICommandHandler<TCommand, TOutput> _handler;

    public AuditLoggingCommandDecorator(ILogger<AuditLoggingCommandDecorator<TCommand, TOutput>> logger,ICommandHandler<TCommand, TOutput> handler)
    {
        _logger = logger;
        _handler = handler;
    }

    public async Task<TOutput> Handle(TCommand command)
    {
        _logger.LogInformation($"BEGIN: Command - {command.GetType().Name};");

        var watch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            var response = await _handler.Handle(command);
            watch.Stop();
            _logger.LogInformation($"COMPLETED: Command (Success) - {command.GetType().Name}; Runtime (ms): {watch.ElapsedMilliseconds}");
            return response;
        }
        catch (DomainValidationException e)
        {
            watch.Stop();
            _logger.LogWarning($"COMPLETED: Command (Warning) - {command.GetType().Name}; Runtime (ms): {watch.ElapsedMilliseconds}", e);
            throw;
        }
        catch (Exception e)
        {
            watch.Stop();
            _logger.LogError($"FAILED: Command - {command.GetType().Name}; Runtime (ms): {watch.ElapsedMilliseconds}", e);
            throw;
        }
    }
}

public sealed class AuditLoggingQueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    private readonly ILogger<AuditLoggingQueryDecorator<TQuery, TResult>> _logger;
    private readonly IQueryHandler<TQuery, TResult> _handler;

    public AuditLoggingQueryDecorator(ILogger<AuditLoggingQueryDecorator<TQuery, TResult>> logger, IQueryHandler<TQuery, TResult> handler)
    {
        _logger = logger;
        _handler = handler;
    }

    public async Task<TResult> Handle(TQuery query)
    {
        _logger.LogInformation($"BEGIN: Query - {query.GetType().Name};");

        var watch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            var response = await _handler.Handle(query);
            watch.Stop();
            _logger.LogInformation($"COMPLETED: Query (Success) - {query.GetType().Name}; Runtime (ms): {watch.ElapsedMilliseconds}");
            return response;
        }
        catch (DomainValidationException e)
        {
            watch.Stop();
            _logger.LogWarning($"COMPLETED: Query (Warnings) - {query.GetType().Name}; Runtime (ms): {watch.ElapsedMilliseconds}", e);
            throw;
        }
        catch (Exception e)
        {
            watch.Stop();
            _logger.LogError($"FAILED: Query - {query.GetType().Name}; Runtime (ms): {watch.ElapsedMilliseconds}", e);
            throw;
        }
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class AuditLogPipelineAttribute : Attribute
{
    public AuditLogPipelineAttribute()
    {
    }
}