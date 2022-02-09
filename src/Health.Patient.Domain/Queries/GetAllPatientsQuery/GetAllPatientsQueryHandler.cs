using Health.Patient.Domain.Core.Decorators;
using Health.Patient.Domain.Core.Models;
using Health.Patient.Domain.Queries.Core;
using Health.Patient.Storage;
using Health.Patient.Storage.Sql;

namespace Health.Patient.Domain.Queries.GetAllPatientsQuery;

[AuditLogPipeline]
[ValidationPipeline]
public sealed class GetAllPatientsQueryHandler : IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientRecord>>
{
    private readonly IPatientUnitOfWork _unitOfWork;
    public GetAllPatientsQueryHandler(IPatientUnitOfWork unitOfWork)
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