using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Health.Patient.Domain.Core.Exceptions;
using Health.Patient.Domain.Core.Mediator;
using Health.Patient.Domain.Models;
using Health.Patient.Domain.Queries.GetAllPatientsQuery;
using Health.Patient.Transport.Api.Models;
using Health.Patient.Transport.Api.UnitTests.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Health.Patient.Transport.Api.UnitTests.Controllers.PatientController;

public class GetAllPatientsTests : IDisposable
{
    private readonly Api.Controllers.PatientController _controller;
    private readonly Mock<ILogger<Api.Controllers.PatientController>> _logger;
    private readonly Mock<IMediator> _mediator;
    private readonly Faker _faker;

    public GetAllPatientsTests()
    {
        _faker = new Faker();
        _logger = new Mock<ILogger<Api.Controllers.PatientController>>();
        _mediator = new Mock<IMediator>();
        _controller = new Api.Controllers.PatientController(_logger.Object, _mediator.Object);
    }

    [Fact]
    public async Task GetAllPatients_Success()
    {
        //Arrange
        var _patient = new PatientRecord(_faker.Name.FirstName(), _faker.Name.LastName(), _faker.Person.DateOfBirth,
            _faker.Random.Guid());
        _mediator
            .Setup(x => x.SendAsync(It.IsAny<GetAllPatientsQuery>()))
            .ReturnsAsync(() => new List<PatientRecord>() {_patient});

        //Act
        var result = await _controller.GetAllPatients();

        //Assert
        var response =
            TestingExtensions.AssertResponseFromIActionResult<GetPatientApiResponse[]>(StatusCodes.Status200OK, result);
        Assert.Single(response);
        Assert.Equal(response[0].PatientId, response[0].PatientId);
        Assert.Equal(response[0].FirstName, response[0].FirstName);
        Assert.Equal(response[0].LastName, response[0].LastName);
        Assert.Equal(response[0].DateOfBirth, response[0].DateOfBirth);
    }

    [Fact]
    public async Task GetPatient_Failed_ThrowsException()
    {
        //Arrange
        var exceptionText = _faker.Lorem.Text();
        _mediator
            .Setup(x => x.SendAsync(It.IsAny<GetAllPatientsQuery>()))
            .ThrowsAsync(new Exception(exceptionText));

        //Act
        var exception = await Assert.ThrowsAsync<Exception>(async () => await _controller.GetAllPatients());
        Assert.Equal(exceptionText, exception.Message);
    }

    [Fact]
    public async Task GetPatient_Failed_ThrowsDomainValidationException()
    {
        //Arrange
        var exceptionText = _faker.Lorem.Text();
        _mediator
            .Setup(x => x.SendAsync(It.IsAny<GetAllPatientsQuery>()))
            .ThrowsAsync(new DomainValidationException(exceptionText));

        //Act
        var exception = await Assert.ThrowsAsync<DomainValidationException>(async () => await _controller.GetAllPatients());
        Assert.Equal(exceptionText, exception.Message);
    }
    
    public void Dispose()
    {
    }
}