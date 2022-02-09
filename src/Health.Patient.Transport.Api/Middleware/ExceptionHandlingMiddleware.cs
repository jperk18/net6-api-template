using FluentValidation;
using Health.Patient.Api.Core.Serialization;

namespace Health.Patient.Api.Middleware;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IJsonSerializer _serializer;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, IJsonSerializer serializer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }
    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var response = new ApiGenericException(GetTitle(exception), statusCode, exception.Message, GetErrors(exception));
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(_serializer.Serialize(response));
        // await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions(){
        //     PropertyNameCaseInsensitive = true,
        //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        // }));
    }
    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };
    private static string GetTitle(Exception exception) =>
        exception switch
        {
            ValidationException valException => "Validation Failure",
            ApplicationException applicationException => applicationException.Message,
            _ => "Server Error"
        };
    private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
    {
        //IReadOnlyDictionary<string, string[]> errors = null;
        IReadOnlyDictionary<string, string[]> errors = new Dictionary<string, string[]>();
        if (exception is ValidationException validationException)
        {
            errors = validationException.Errors.GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(x => x.Key, x => x.Values);;
        }
        return errors;
    }
}