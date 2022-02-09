using Health.Patient.Api.Requests.Interfaces;

namespace Health.Patient.Api.Requests;

public class CreatePatientApiResponse : IPatientIdentifer
{
    public Guid PatientId { get; set; }
}