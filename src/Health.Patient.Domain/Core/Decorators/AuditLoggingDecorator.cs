using Health.Patient.Domain.Commands.Core;
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
        _logger.LogInformation($"Staring command: {command.GetType().Name}");
        
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _handler.Handle(command);
        watch.Stop();
        
        _logger.LogInformation($"Finished command: {command.GetType().Name}");
        _logger.LogInformation($"Command {command.GetType().Name} runtime (ms): {watch.ElapsedMilliseconds}");
        
        return response;
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
        _logger.LogInformation($"Starting query: {query.GetType().Name}");
        
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _handler.Handle(query);
        watch.Stop();
        
        _logger.LogInformation($"Finished query: {query.GetType().Name}");
        _logger.LogInformation($"Query {query.GetType().Name} runtime (ms): {watch.ElapsedMilliseconds}");

        return response;
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class AuditLogPipelineAttribute : Attribute
{
    public AuditLogPipelineAttribute()
    {
    }
}