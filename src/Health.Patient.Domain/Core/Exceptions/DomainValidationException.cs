using FluentValidation;
using Health.Patient.Domain.Core.Exceptions.Models;
using Severity = Health.Patient.Domain.Core.Exceptions.Models.Severity;

namespace Health.Patient.Domain.Core.Exceptions;

public class DomainValidationException : Exception, IDomainValidation
{
    public DomainValidationException(string message)
    {
        Message = message;
        Errors = new List<ValidationFailure>();
    }

    public DomainValidationException(string message, IEnumerable<ValidationFailure>? errors)
    {
        Message = message;
        Errors = errors;
    }
    
    public DomainValidationException(ValidationException e)
    {
        Message = e.Message;
        Errors = e.Errors.Select(x => new ValidationFailure(x.ErrorMessage)
        {
            AttemptedValue = x.AttemptedValue,
            ErrorCode = x.ErrorCode,
            PropertyName = x.PropertyName,
            Severity = x.Severity == FluentValidation.Severity.Error ? Severity.Error :
                x.Severity == FluentValidation.Severity.Info ? Severity.Info :
                x.Severity == FluentValidation.Severity.Warning ? Severity.Warning : Severity.Error
        }).ToList();
    }

    public DomainValidation ToDomainValidation()
    {
        return new DomainValidation(Message)
        {
            Errors = Errors
        };
    }

    public IEnumerable<ValidationFailure>? Errors { get; set; }
    public override string Message { get; }
}