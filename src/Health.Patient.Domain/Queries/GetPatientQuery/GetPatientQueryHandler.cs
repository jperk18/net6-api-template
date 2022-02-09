using FluentValidation;
using Health.Patient.Domain.Core.Decorators;
using Health.Patient.Domain.Core.Models;
using Health.Patient.Domain.Queries.Core;
using Health.Patient.Storage;
using Health.Patient.Storage.Sql;

namespace Health.Patient.Domain.Queries.GetPatientQuery;

[AuditLogPipeline]
[ValidationPipeline]
public sealed class GetPatientQueryHandler : IQueryHandler<GetPatientQuery, PatientRecord>
{
    private readonly IPatientUnitOfWork _unitOfWork;
    public GetPatientQueryHandler(IPatientUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<PatientRecord> Handle(GetPatientQuery command)
    {
        //TODO: More Business logic
        
        var i = await _unitOfWork.Patients.GetById(command.PatientId);

        if (i == null)
            throw new ValidationException("Record does not exist");
        
        return new PatientRecord(i.FirstName, i.LastName, i.DateOfBirth, i.Id);
    }
}