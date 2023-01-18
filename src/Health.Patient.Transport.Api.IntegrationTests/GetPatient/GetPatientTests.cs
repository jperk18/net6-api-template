using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using Health.Patient.Transport.Api.IntegrationTests.Core;
using Health.Patient.Transport.Api.IntegrationTests.Core.Seeds;
using Health.Patient.Transport.Api.Middleware;
using Health.Patient.Transport.Api.Models;
using Microsoft.AspNetCore.WebUtilities;
using Xunit;

namespace Health.Patient.Transport.Api.IntegrationTests.GetPatient;

public class GetPatientTests
{
    private const string ApiUri = "/api/Patient";
    private readonly Faker _faker;
    
    public GetPatientTests()
    {
        _faker = new Faker();
    }
    
    private string BuildUri(GetPatientApiRequest req) => QueryHelpers.AddQueryString(ApiUri, new Dictionary<string, string>()
    {
        {"PatientId", req.PatientId.ToString() }
    }!);
    
    [Fact]
    public async Task GetPatient_Success()
    {
        //Arrange
        var seed = new GenerateValidPatientsDataSeed(_faker.Random.Number(1, 50));
        var application = new PatientApiApplication(new List<IDataSeed>(){ seed });
        var client = application.CreateClient();
        var patient = seed.Patients?.First();
        var request = new GetPatientApiRequest(patient!.Id);
        
        //Act
        var response = await client.GetAsync(BuildUri(request));
        response.EnsureSuccessStatusCode();
        
        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<GetPatientApiResponse>();
        Assert.NotNull(result);
        Assert.Equal(patient.Id, result!.PatientId);
        Assert.Equal(patient.FirstName, result!.FirstName);
        Assert.Equal(patient.LastName, result!.LastName);
        Assert.Equal(patient.DateOfBirth, result!.DateOfBirth);
    }
    
    [Fact]
    public async Task GetPatient_Failed_PatientDoesNotExist()
    {
        //Arrange
        var seed = new GenerateValidPatientsDataSeed(_faker.Random.Number(1, 50));
        var application = new PatientApiApplication(new List<IDataSeed>(){ seed });
        var client = application.CreateClient();
        var request = new GetPatientApiRequest(_faker.Random.Guid());
        
        //Act
        var response = await client.GetAsync(BuildUri(request));
        
        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiGenericValidationResultObject>();
        Assert.NotNull(result);
        Assert.Equal("Record does not exist", result!.Detail);
    }
    
    [Fact]
    public async Task GetPatient_Failed_InvalidPatientId()
    {
        //Arrange
        var seed = new GenerateValidPatientsDataSeed(_faker.Random.Number(1, 50));
        var application = new PatientApiApplication(new List<IDataSeed>(){ seed });
        var client = application.CreateClient();
        var request = new GetPatientApiRequest(Guid.Empty);
        
        //Act
        var response = await client.GetAsync(BuildUri(request));
        
        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiGenericValidationResultObject>();
        Assert.NotNull(result);
        var error = Assert.Single(result!.Errors);
        Assert.Equal(nameof(request.PatientId), error.Key);
    }
}