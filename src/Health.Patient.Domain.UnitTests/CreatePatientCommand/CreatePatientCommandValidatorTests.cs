using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Xunit;

namespace Health.Patient.Domain.UnitTests.CreatePatientCommand;

public class CreatePatientCommandValidatorTests
{
    private readonly CreatePatientCommandValidator _controller;
    private readonly Faker _faker;

    public CreatePatientCommandValidatorTests()
    {
        _faker = new Faker();
        _controller = new CreatePatientCommandValidator();
    }

    [Fact]
    public async Task CreatePatientCommandValidator_Success()
    {
        //Arrange
        var request = new Commands.CreatePatientCommand.CreatePatientCommand(_faker.Person.FirstName,
            _faker.Person.LastName, _faker.Person.DateOfBirth);

        //Act
        var result = await _controller.ValidateAsync(request);

        //Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task CreatePatientCommandValidator_Invalid_FirstName()
    {
        //Arrange
        var request = new Commands.CreatePatientCommand.CreatePatientCommand(string.Empty,
            _faker.Person.LastName, _faker.Person.DateOfBirth);

        //Act
        var result = await _controller.ValidateAsync(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(nameof(request.FirstName), result.Errors.First().PropertyName);
    }

    [Fact]
    public async Task CreatePatientCommandValidator_Invalid_LastName()
    {
        //Arrange
        var request = new Commands.CreatePatientCommand.CreatePatientCommand(_faker.Person.FirstName, string.Empty,
            _faker.Person.DateOfBirth);

        //Act
        var result = await _controller.ValidateAsync(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(nameof(request.LastName), result.Errors.First().PropertyName);
    }
}