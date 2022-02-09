using Health.Patient.Domain.Core.Models.Interfaces;

namespace Health.Patient.Domain.Core.Models;

public class PatientRecord : IPatientRecord
{
    public PatientRecord(string firstName, string lastName, DateTime dateOfBirth, Guid patientId)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Id = patientId;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Guid Id { get; set; }
}