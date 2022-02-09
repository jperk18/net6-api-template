using Health.Patient.Domain.Core.Models;
using Health.Patient.Domain.Queries.Core;

namespace Health.Patient.Domain.Queries.GetAllPatientsQuery;

public sealed class GetAllPatientsQuery : IQuery<IEnumerable<PatientRecord>>
{
}