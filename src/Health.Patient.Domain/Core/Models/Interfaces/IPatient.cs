namespace Health.Patient.Domain.Core.Models.Interfaces;

public interface IPatient
{
    string FirstName { get; set; }
    string LastName { get; set; }
    DateTime DateOfBirth { get; set; }
}