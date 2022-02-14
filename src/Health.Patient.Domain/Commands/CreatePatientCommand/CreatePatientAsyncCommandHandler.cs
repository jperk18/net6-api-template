using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Core.Decorators;
using Health.Patient.Storage.Sql;

namespace Health.Patient.Domain.Commands.CreatePatientCommand;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
[TransactionPipeline]
public sealed class CreatePatientAsyncCommandHandler : IAsyncCommandHandler<CreatePatientCommand, Guid>
{
    private readonly IPatientUnitOfWork _unitOfWork;
    public CreatePatientAsyncCommandHandler(IPatientUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<Guid> Handle(CreatePatientCommand command)
    {
        //TODO: More Business logic
        var p = await _unitOfWork.Patients.Add(new Storage.Sql.Core.Databases.PatientDb.Models.Patient(
            Guid.NewGuid(), command.FirstName, command.LastName, command.DateOfBirth
        ));

        await _unitOfWork.Complete();
        return p.Id;
    }
}