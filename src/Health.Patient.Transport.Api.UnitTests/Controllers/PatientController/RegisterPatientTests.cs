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
    private readonly Mock<ICommandHandler<CreatePatientCommand, Guid>> _createPatientHandler;
    private readonly Mock<IQueryHandler<GetPatientQuery, PatientRecord>> _getPatientHandler;
    private readonly Mock<IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientRecord>>> _getAllPatientsHandler;
    private readonly Faker _faker;

    public RegisterPatientTests()
    {
        _faker = new Faker();
        _logger = new Mock<ILogger<Api.Controllers.PatientController>>();
        _createPatientHandler = new Mock<ICommandHandler<CreatePatientCommand, Guid>>();
        _getPatientHandler = new Mock<IQueryHandler<GetPatientQuery, PatientRecord>>();
        _getAllPatientsHandler = new Mock<IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientRecord>>>();
        _controller = new Api.Controllers.PatientController(_logger.Object, _createPatientHandler.Object, _getPatientHandler.Object,
            _getAllPatientsHandler.Object);
    }

    [Theory]
    [MemberData(nameof(RegisterPatient_Success_Data))]
    public async Task RegisterPatient_Success(CreatePatientApiRequest request)
    {
        TestingExtensions.TestHandler(async () => {
            //Arrange
            var patientId = _faker.Random.Guid();
            _createPatientHandler
                .Setup(x => x.Handle(It.IsAny<CreatePatientCommand>()))
                .ReturnsAsync(() => patientId);
            
            //Act
            var result = await _controller.RegisterPatient(request);
            
            //Assert
            var response = TestingExtensions.AssertResponseFromIActionResult<CreatePatientApiResponse>(StatusCodes.Status201Created, result);
            Assert.Equal(patientId, response.PatientId);
        });
    }
    
    [Theory]
    [MemberData(nameof(RegisterPatient_Fail_EmptyData))]
    public async Task RegisterPatient_Failed_ThrowsDomainValidation(CreatePatientApiRequest request)
    {
        TestingExtensions.TestHandler(async () => {
            //Arrange
            _createPatientHandler
                .Setup(x => x.Handle(It.IsAny<CreatePatientCommand>()))
                .ThrowsAsync(new DomainValidationException(_faker.Lorem.Text()));
            
            //Act
            await Assert.ThrowsAsync<DomainValidationException>(async() => await _controller.RegisterPatient(request));
        });
    }
    
    [Fact]
    public async Task RegisterPatient_Failed_ThrowsException()
    {
        TestingExtensions.TestHandler(async () => {
            //Arrange
            var request = new CreatePatientApiRequest(_faker.Name.FirstName(), _faker.Name.LastName(), _faker.Person.DateOfBirth);
            
            _createPatientHandler
                .Setup(x => x.Handle(It.IsAny<CreatePatientCommand>()))
                .ThrowsAsync(new Exception(_faker.Lorem.Text()));
            
            //Act
            await Assert.ThrowsAsync<DomainValidationException>(async() => await _controller.RegisterPatient(request));
        });
    }

    private static IEnumerable<object[]> RegisterPatient_Success_Data()
    {
        var faker = new Faker<CreatePatientApiRequest>()
            .RuleFor(o => o.FirstName, f => f.Person.FirstName)
            .RuleFor(o => o.LastName, f => f.Person.LastName)
            .RuleFor(o => o.DateOfBirth, f => f.Person.DateOfBirth);
;
        var allData = faker.Generate(5);

        return allData.Select(x => new object[] {x});
    }

    private static IEnumerable<object[]> RegisterPatient_Fail_EmptyData()
    {
        var faker = new Faker<CreatePatientApiRequest>()
            .RuleFor(o => o.FirstName, f => f.Person.FirstName)
            .RuleFor(o => o.LastName, f => f.Person.LastName)
            .RuleFor(o => o.DateOfBirth, f => f.Person.DateOfBirth);

        //One that doesn't exist for example
        var data = faker.Generate(2);
        
        var allData = new List<CreatePatientApiRequest>()
        {
            new(string.Empty, data[0].LastName, data[0].DateOfBirth),
            data[0]
        };

        return allData.Select(x => new object[] {x});
    }
    
    public void Dispose()
    {
    }
}