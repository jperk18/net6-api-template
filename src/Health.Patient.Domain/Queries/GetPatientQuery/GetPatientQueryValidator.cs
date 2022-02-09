using FluentValidation;

namespace Health.Patient.Domain.Queries.GetPatientQuery;

public class GetPatientQueryValidator : AbstractValidator<GetPatientQuery>
{
    public GetPatientQueryValidator()
    {
        RuleFor(patient => patient.PatientId).NotNull().NotEmpty();
    }
}