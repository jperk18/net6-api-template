using Health.Patient.Domain.Core.Models;
using Health.Patient.Domain.Queries.Core;

namespace Health.Patient.Domain.Queries.GetPatientQuery;

public sealed class GetPatientQuery : IQuery<PatientRecord>
{
    public GetPatientQuery(Guid PatientId)
    {
        this.PatientId = PatientId;
    }
    public Guid PatientId { get; }
}