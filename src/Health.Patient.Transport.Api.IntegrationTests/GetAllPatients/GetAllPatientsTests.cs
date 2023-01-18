using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using Health.Patient.Transport.Api.IntegrationTests.Core;
using Health.Patient.Transport.Api.IntegrationTests.Core.Seeds;
using Health.Patient.Transport.Api.Models;
using Xunit;

namespace Health.Patient.Transport.Api.IntegrationTests.GetAllPatients;

public class GetAllPatientsTests
{
    private readonly string apiUri = "/api/Patient/All";
    private readonly Faker _faker;
    
    public GetAllPatientsTests()
    {
        _faker = new Faker();
    }
    
    private string BuildUri() => apiUri;
    
    [Fact]
    public async Task GetAllPatients_Success()
    {
        //Arrange
        var seed = new GenerateValidPatientsDataSeed(_faker.Random.Number(1, 50));
        var application = new PatientApiApplication(new List<IDataSeed>(){ seed });
        var client = application.CreateClient();

        //Act
        var response = await client.GetAsync(BuildUri());
        response.EnsureSuccessStatusCode();
        
        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<List<GetPatientApiResponse>>();
        Assert.Equal(seed.Patients!.Count(), result!.Count);
        Assert.True(!result.Select(x => x.PatientId).Except(seed.Patients!.Select(c => c.Id)).Any());
    }
}