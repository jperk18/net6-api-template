namespace Health.Patient.Transport.Api.Core;

public interface IApiConfiguration
{
    string? SomeValue { get; set; }
}

public class ApiConfiguration : IApiConfiguration
{
    public string? SomeValue { get; set; }
}