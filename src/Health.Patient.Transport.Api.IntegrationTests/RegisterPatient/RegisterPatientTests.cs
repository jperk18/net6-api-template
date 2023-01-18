using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Health.Patient.Transport.Api.IntegrationTests.Core;
using Health.Patient.Transport.Api.Middleware;
using Health.Patient.Transport.Api.Models;
using Xunit;

namespace Health.Patient.Transport.Api.IntegrationTests.RegisterPatient;

public class RegisterPatientTests
{
    private readonly string apiUri = "/api/Patient";
    private readonly Faker _faker;
    
    public RegisterPatientTests()
    {
        _faker = new Faker();
    }

    private string BuildUri() => apiUri;
    
    [Fact]
    public async Task RegisterPatient_Success()
    {
        //Arrange
        var application = new PatientApiApplication();
        var client = application.CreateClient();
        var request = new CreatePatientApiRequest(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth);
        
        //Act
        var buffer = Encoding.UTF8.GetBytes(Core.Extensions.JsonSerializer.Serialize(request));
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await client.PostAsync(apiUri, byteContent);
        response.EnsureSuccessStatusCode();
        
        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<CreatePatientApiResponse>();
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetPatient_Failed_InvalidPatientId()
    {
        //Arrange
        var application = new PatientApiApplication();
        var client = application.CreateClient();
        var request = new CreatePatientApiRequest(string.Empty, string.Empty, _faker.Person.DateOfBirth);
        
        //Act
        var buffer = Encoding.UTF8.GetBytes(Core.Extensions.JsonSerializer.Serialize(request));
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await client.PostAsync(apiUri, byteContent);
        
        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiGenericValidationResultObject>();
        Assert.NotNull(result);
        Assert.NotNull(result!.Errors);
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains(result.Errors, f => f.Key == nameof(request.FirstName));
        Assert.Contains(result.Errors, f => f.Key == nameof(request.LastName));
    }
    
    [Fact]
    public async Task GetPatient_Failed_InvalidRequest()
    {
        //Arrange
        var application = new PatientApiApplication();
        var client = application.CreateClient();
        var request = new CreatePatientApiRequest(null!, string.Empty, _faker.Person.DateOfBirth);
        
        //Act
        var buffer = Encoding.UTF8.GetBytes(Core.Extensions.JsonSerializer.Serialize(request));
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await client.PostAsync(apiUri, byteContent);
        
        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}