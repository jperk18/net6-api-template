using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Health.Patient.Api.Requests;
using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Core.Exceptions;
using Health.Patient.Domain.Core.Models;
using Health.Patient.Domain.Queries.Core;
using Health.Patient.Domain.Queries.GetAllPatientsQuery;
using Health.Patient.Domain.Queries.GetPatientQuery;
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
    private readonly Mock<ICommandHandler<CreatePatientCommand, Guid>> _createPatientHandler;
    private readonly Mock<IQueryHandler<GetPatientQuery, PatientRecord>> _getPatientHandler;
    private readonly Mock<IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientRecord>>> _getAllPatientsHandler;
    private readonly Faker _faker;

    public GetAllPatientsTests()
    {
        _faker = new Faker();
        _logger = new Mock<ILogger<Api.Controllers.PatientController>>();
        _createPatientHandler = new Mock<ICommandHandler<CreatePatientCommand, Guid>>();
        _getPatientHandler = new Mock<IQueryHandler<GetPatientQuery, PatientRecord>>();
        _getAllPatientsHandler = new Mock<IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientRecord>>>();
        _controller = new Api.Controllers.PatientController(_logger.Object, _createPatientHandler.Object, _getPatientHandler.Object,
            _getAllPatientsHandler.Object);
    }

    [Fact]
    public async Task GetAllPatients_Success()
    {
        TestingExtensions.TestHandler(async () => {
            //Arrange
            var _patient = new PatientRecord(_faker.Name.FirstName(), _faker.Name.LastName(), _faker.Person.DateOfBirth,
                _faker.Random.Guid());
            _getAllPatientsHandler
                .Setup(x => x.Handle(It.IsAny<GetAllPatientsQuery>()))
                .ReturnsAsync(() => new List<PatientRecord>(){ _patient });
            
            //Act
            var result = await _controller.GetAllPatients();
            
            //Assert
            var response = TestingExtensions.AssertResponseFromIActionResult<GetPatientApiResponse[]>(StatusCodes.Status200OK, result);
            Assert.Single(response);
            Assert.Equal(response[0].PatientId, response[0].PatientId);
            Assert.Equal(response[0].FirstName, response[0].FirstName);
            Assert.Equal(response[0].LastName, response[0].LastName);
            Assert.Equal(response[0].DateOfBirth, response[0].DateOfBirth);
        });
    }
    
    [Fact]
    public async Task GetPatient_Failed_ThrowsException()
    {
        TestingExtensions.TestHandler(async () => {
            //Arrange
            var request = new GetPatientApiRequest();
            
            _getAllPatientsHandler
                .Setup(x => x.Handle(It.IsAny<GetAllPatientsQuery>()))
                .ThrowsAsync(new Exception(_faker.Lorem.Text()));
            
            //Act
            await Assert.ThrowsAsync<DomainValidationException>(async() => await _controller.GetAllPatients());
        });
    }

    public void Dispose()
    {
    }
}