namespace Health.Patient.Domain.Core.Exceptions.Models;

public class DomainValidation : IDomainValidation
{
    public DomainValidation(string message)
    {
        Message = message;
    }

    public IEnumerable<ValidationFailure>? Errors { get; set; }
    public string Message { get; set; }
    
    public DomainValidationException ToDomainValidationException()
    {
        return new DomainValidationException(Message, Errors);
    }
}