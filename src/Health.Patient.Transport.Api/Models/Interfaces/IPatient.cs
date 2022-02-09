namespace Health.Patient.Api.Requests.Interfaces;

public interface IPatient
{
    string FirstName { get; }
    string LastName { get; }
    DateTime DateOfBirth { get; }
}