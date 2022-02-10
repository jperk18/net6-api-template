namespace Health.Patient.Domain.Core.Exceptions.Models;

public interface IDomainValidation
{
    IEnumerable<ValidationFailure>? Errors { get; set; }
}