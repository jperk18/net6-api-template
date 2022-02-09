using FluentValidation;

namespace Health.Patient.Domain.Commands.CreatePatientCommand;

public sealed class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientCommandValidator()
    {
        RuleFor(patient => patient.FirstName).NotNull().NotEmpty();
        RuleFor(patient => patient.LastName).NotNull().NotEmpty();
    }
}