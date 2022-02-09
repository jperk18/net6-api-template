using Health.Patient.Api.Requests.Interfaces;

namespace Health.Patient.Api.Requests;

public class CreatePatientApiRequest : IPatient
{
    public CreatePatientApiRequest(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public DateTime DateOfBirth { get; }
}