using Health.Patient.Transport.Api.Models.Interfaces;

namespace Health.Patient.Transport.Api.Models;

public class GetPatientApiRequest : IPatientIdentifer
{
    public Guid PatientId { get; set; }
}