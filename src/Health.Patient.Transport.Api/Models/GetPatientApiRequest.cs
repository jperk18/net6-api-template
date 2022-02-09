using Health.Patient.Api.Requests.Interfaces;

namespace Health.Patient.Api.Requests;

public class GetPatientApiRequest : IPatientIdentifer
{
    public Guid PatientId { get; set; }
}