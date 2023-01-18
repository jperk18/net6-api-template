using Health.Patient.Domain.Core.Mediator.Queries;
using Health.Patient.Domain.Models;

namespace Health.Patient.Domain.Queries.GetAllPatientsQuery;

public sealed class GetAllPatientsQuery : IQuery<IEnumerable<PatientRecord>>
{
}