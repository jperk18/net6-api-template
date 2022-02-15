using System.Text.Json;

namespace Health.Patient.Transport.Api.IntegrationTests.Core.Extensions;

public static class JsonSerializer
{
    private static readonly JsonSerializerOptions _settings = new JsonSerializerOptions()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        IgnoreNullValues = false
    };
    
    public static string Serialize(object value)
    {
        return System.Text.Json.JsonSerializer.Serialize(value, _settings);
    }
    
    public static T? Deserialize<T>(string value)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(value, _settings) ?? default(T);
    }
}