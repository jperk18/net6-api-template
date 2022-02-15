using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Health.Patient.Domain.Queries.GetPatientQuery;
using Xunit;

namespace Health.Patient.Domain.UnitTests.GetPatientQuery;

public class GetPatientQueryValidatorTests
{
    private readonly GetPatientQueryValidator _controller;
    private readonly Faker _faker;

    public GetPatientQueryValidatorTests()
    {
        _faker = new Faker();
        _controller = new GetPatientQueryValidator();
    }

    [Fact]
    public async Task GetPatientQueryValidator_Success()
    {
        //Arrange
        var request = new Queries.GetPatientQuery.GetPatientQuery(_faker.Random.Guid());

        //Act
        var result = await _controller.ValidateAsync(request);

        //Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task CreatePatientCommandValidator_Invalid_FirstName()
    {
        //Arrange
        var request = new Queries.GetPatientQuery.GetPatientQuery(new Guid());

        //Act
        var result = await _controller.ValidateAsync(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(nameof(request.PatientId), result.Errors.First().PropertyName);
    }
}