namespace Health.Patient.Domain.Core.Exceptions.Models;

public class ValidationFailure
{
    public ValidationFailure()
    {
        
    }
    public ValidationFailure(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// The name of the property.
    /// </summary>
    public string? PropertyName { get; set; }

    /// <summary>
    /// The error message
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// The property value that caused the failure.
    /// </summary>
    public object? AttemptedValue { get; set; }
    
    /// <summary>
    /// Custom severity level associated with the failure.
    /// </summary>
    public Severity Severity { get; set; } = Severity.Error;

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string ErrorCode { get; set; }
}