using Health.Patient.Domain.Commands.Core;
using Health.Patient.Storage.Sql.Core.Databases.PatientDb;
using Microsoft.Extensions.Logging;

namespace Health.Patient.Domain.Core.Decorators;

public sealed class TransactionCommandDecorator<TCommand, TOutput> : ICommandHandler<TCommand, TOutput>
    where TCommand : ICommand<TOutput>
{
    private readonly ILogger<TransactionCommandDecorator<TCommand, TOutput>> _logger;
    private readonly ICommandHandler<TCommand, TOutput> _handler;
    private readonly PatientDbContext _dbContext;

    public TransactionCommandDecorator(ILogger<TransactionCommandDecorator<TCommand, TOutput>> logger,ICommandHandler<TCommand, TOutput> handler, PatientDbContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<TOutput> Handle(TCommand command)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var response = await _handler.Handle(command);
            await transaction.CommitAsync();
            return response;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class TransactionPipelineAttribute : Attribute
{
    public TransactionPipelineAttribute()
    {
    }
}