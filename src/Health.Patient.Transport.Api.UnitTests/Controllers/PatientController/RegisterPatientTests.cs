using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Core.Exceptions;
using Health.Patient.Domain.Core.Mediator;
using Health.Patient.Transport.Api.Models;
using Health.Patient.Transport.Api.UnitTests.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Health.Patient.Transport.Api.UnitTests.Controllers.PatientController;

public class RegisterPatientTests : IDisposable
{
    private readonly Api.Controllers.PatientController _controller;
    private readonly Mock<ILogger<Api.Controllers.PatientController>> _logger;
    private readonly Mock<IMediator> _mediator;
    private readonly Faker _faker;

    public RegisterPatientTests()
    {
        _faker = new Faker();
        _logger = new Mock<ILogger<Api.Controllers.PatientController>>();
        _mediator = new Mock<IMediator>();
        _controller = new Api.Controllers.PatientController(_logger.Object, _mediator.Object);
    }

    [Theory]
    [MemberData(nameof(RegisterPatient_Success_Data))]
    public async Task RegisterPatient_Success(CreatePatientApiRequest request)
    {
        //Arrange
        var patientId = _faker.Random.Guid();
        _mediator
            .Setup(x => x.SendAsync(It.IsAny<CreatePatientCommand>()))
            .ReturnsAsync(() => patientId);

        //Act
        var result = await _controller.RegisterPatient(request);

        //Assert
        var response =
            TestingExtensions.AssertResponseFromIActionResult<CreatePatientApiResponse>(StatusCodes.Status201Created,
                result);
        Assert.Equal(patientId, response.PatientId);
    }
    private static IEnumerable<object[]> RegisterPatient_Success_Data()
    {
        var faker = new Faker<CreatePatientApiRequest>()
                .CustomInstantiator(x => new CreatePatientApiRequest(x.Person.FirstName, x.Person.LastName, x.Person.DateOfBirth))
        ;
        var allData = faker.Generate(5).Select(x => (CreatePatientApiRequest)x);

        return allData.Select(x => new object[] {x});
    }

    [Fact]
    public async Task RegisterPatient_Failed_ThrowsDomainValidation()
    {
        //Arrange
        var request = new CreatePatientApiRequest(_faker.Name.FirstName(), _faker.Name.LastName(),
            _faker.Person.DateOfBirth);
        var exceptionText = _faker.Lorem.Text();
        _mediator
            .Setup(x => x.SendAsync(It.IsAny<CreatePatientCommand>()))
            .ThrowsAsync(new DomainValidationException(exceptionText));

        //Act
        var exception = await Assert.ThrowsAsync<DomainValidationException>(async () => await _controller.RegisterPatient(request));
        Assert.Equal(exceptionText, exception.Message);
    }

    [Fact]
    public async Task RegisterPatient_Failed_ThrowsException()
    {
        //Arrange
        var request = new CreatePatientApiRequest(_faker.Name.FirstName(), _faker.Name.LastName(),
            _faker.Person.DateOfBirth);
        var exceptionText = _faker.Lorem.Text();

        _mediator
            .Setup(x => x.SendAsync(It.IsAny<CreatePatientCommand>()))
            .ThrowsAsync(new Exception(exceptionText));

        //Act
        var exception = await Assert.ThrowsAsync<Exception>(async () => await _controller.RegisterPatient(request));
        Assert.Equal(exceptionText, exception.Message);
    }

    public void Dispose()
    {
    }
}