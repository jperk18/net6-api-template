using Health.Patient.Domain.Core.Mediator.Queries;
using Health.Patient.Domain.Models;

namespace Health.Patient.Domain.Queries.GetPatientQuery;

public sealed class GetPatientQuery : IQuery<PatientRecord>
{
    public GetPatientQuery(Guid PatientId)
    {
        this.PatientId = PatientId;
    }
    public Guid PatientId { get; }
}