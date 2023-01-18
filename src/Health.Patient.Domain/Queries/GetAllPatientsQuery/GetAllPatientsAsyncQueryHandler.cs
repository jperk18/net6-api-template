using Health.Patient.Domain.Core.Mediator.Decorators;
using Health.Patient.Domain.Core.Mediator.Queries;
using Health.Patient.Domain.Models;
using Health.Patient.Storage;
using Health.Patient.Storage.Sql;

namespace Health.Patient.Domain.Queries.GetAllPatientsQuery;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
public sealed class GetAllPatientsAsyncQueryHandler : IAsyncQueryHandler<GetAllPatientsQuery, IEnumerable<PatientRecord>>
{
    private readonly IPatientUnitOfWork _unitOfWork;
    public GetAllPatientsAsyncQueryHandler(IPatientUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<IEnumerable<PatientRecord>> Handle(GetAllPatientsQuery command)
    {
        return await Task.FromResult(
            _unitOfWork.Patients.GetAll().Select(i => new PatientRecord(i.FirstName, i.LastName, i.DateOfBirth, i.Id))
            );
    }
}