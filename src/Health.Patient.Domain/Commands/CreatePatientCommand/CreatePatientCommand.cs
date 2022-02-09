using Health.Patient.Domain.Commands.Core;

namespace Health.Patient.Domain.Commands.CreatePatientCommand;

public sealed class CreatePatientCommand: ICommand<Guid>
{
    public CreatePatientCommand(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}