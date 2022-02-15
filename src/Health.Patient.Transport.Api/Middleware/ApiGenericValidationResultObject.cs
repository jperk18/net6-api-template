namespace Health.Patient.Transport.Api.Middleware;

public class ApiGenericValidationResultObject
{
    public string Title { get; }
    public int Status { get; }
    public string Detail { get; }
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ApiGenericValidationResultObject(string title, int status, string detail, IReadOnlyDictionary<string, string[]> errors)
    {
        this.Title = title;
        this.Status = status;
        this.Detail = detail;
        this.Errors = errors;
    }
}