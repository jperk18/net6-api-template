using Health.Patient.Domain.Core.Mediator.Commands;
using Health.Patient.Domain.Core.Mediator.Queries;

namespace Health.Patient.Domain.Core.Mediator;

public sealed class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<TOutput> SendAsync<TOutput>(ICommand<TOutput> command)
    {
        var type = typeof(IAsyncCommandHandler<,>);
        var argTypes = new Type[] {command.GetType(), typeof(TOutput)};
        var handlerType = type.MakeGenericType(argTypes);
        dynamic handler = _serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException("No handler found for command");
        TOutput result = await handler?.Handle((dynamic) command)!;
        return result;
    }
    
    public async Task<TResult> SendAsync<TResult>(IQuery<TResult> query)
    {
        var type = typeof(IAsyncQueryHandler<,>);
        var argTypes = new Type[] {query.GetType(), typeof(TResult)};
        var handlerType = type.MakeGenericType(argTypes);
        dynamic handler = _serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException("No handler found for query");
        TResult result = await handler?.Handle((dynamic) query)!;
        return result;
    } 
}