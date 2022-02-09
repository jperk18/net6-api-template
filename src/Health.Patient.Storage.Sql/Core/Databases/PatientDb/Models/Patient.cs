using System.ComponentModel.DataAnnotations;

namespace Health.Patient.Storage.Sql.Core.Databases.PatientDb.Models;

public class Patient
{
    public Patient(Guid id, string firstName, string lastName, DateTime dateOfBirth)
    {
        Id = id;
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        DateOfBirth = dateOfBirth;
    }

    [Key]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}