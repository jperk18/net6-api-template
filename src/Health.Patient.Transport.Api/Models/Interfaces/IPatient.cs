namespace Health.Patient.Transport.Api.Models.Interfaces;

public interface IPatient
{
    string FirstName { get; }
    string LastName { get; }
    DateTime DateOfBirth { get; }
}