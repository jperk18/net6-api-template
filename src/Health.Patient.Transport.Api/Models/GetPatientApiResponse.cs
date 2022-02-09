using Health.Patient.Api.Requests.Interfaces;

namespace Health.Patient.Api.Requests;

public class GetPatientApiResponse : IPatient, IPatientIdentifer
{
    public GetPatientApiResponse(Guid patientId, string firstName, string lastName, DateTime dateOfBirth)
    {
        PatientId = patientId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public Guid PatientId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime DateOfBirth { get; }
}