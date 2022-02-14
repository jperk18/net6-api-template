using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Health.Patient.Domain.Core.Exceptions;
using Health.Patient.Domain.Core.Mediator;
using Health.Patient.Domain.Core.Models;
using Health.Patient.Domain.Queries.GetPatientQuery;
using Health.Patient.Transport.Api.Models;
using Health.Patient.Transport.Api.UnitTests.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Health.Patient.Transport.Api.UnitTests.Controllers.PatientController;

public class GetPatientTests : IDisposable
{
    private readonly Api.Controllers.PatientController _controller;
    private readonly Mock<ILogger<Api.Controllers.PatientController>> _logger;
    private readonly Mock<IMediator> _mediator;
    private readonly Faker _faker;

    public GetPatientTests()
    {
        _faker = new Faker();
        _logger = new Mock<ILogger<Api.Controllers.PatientController>>();
        _mediator = new Mock<IMediator>();
    }

    [Theory]
    [MemberData(nameof(GetPatient_Success_Data))]
    public async Task GetPatient_Success(GetPatientApiRequest request)
    {
        TestingExtensions.TestHandler(async () => {
            //Arrange
            _mediator
                .Setup(x => x.SendAsync(It.IsAny<GetPatientQuery>()))
                .ReturnsAsync(() => new PatientRecord(_faker.Name.FirstName(), _faker.Name.LastName(), _faker.Person.DateOfBirth, _faker.Random.Guid()));
            
            //Act
            var result = await _controller.GetPatient(request);
            
            //Assert
            var response = TestingExtensions.AssertResponseFromIActionResult<GetPatientApiResponse>(StatusCodes.Status200OK, result);
            Assert.Equal(response.PatientId, response.PatientId);
            Assert.Equal(response.FirstName, response.FirstName);
            Assert.Equal(response.LastName, response.LastName);
            Assert.Equal(response.DateOfBirth, response.DateOfBirth);
        });
    }
    
    [Theory]
    [MemberData(nameof(GetPatient_Fail_EmptyData))]
    public async Task GetPatient_Failed_ThrowsDomainValidation(GetPatientApiRequest request)
    {
        TestingExtensions.TestHandler(async () => {
            //Arrange
            _mediator
                .Setup(x => x.SendAsync(It.IsAny<GetPatientQuery>()))
                .ThrowsAsync(new DomainValidationException(_faker.Lorem.Text()));
            
            //Act
            await Assert.ThrowsAsync<DomainValidationException>(async() => await _controller.GetPatient(request));
        });
    }
    
    [Fact]
    public async Task GetPatient_Failed_ThrowsException()
    {
        TestingExtensions.TestHandler(async () => {
            //Arrange
            var request = new GetPatientApiRequest();
            
            _mediator
                .Setup(x => x.SendAsync(It.IsAny<GetPatientQuery>()))
                .ThrowsAsync(new Exception(_faker.Lorem.Text()));
            
            //Act
            await Assert.ThrowsAsync<DomainValidationException>(async() => await _controller.GetPatient(request));
        });
    }

    private static IEnumerable<object[]> GetPatient_Success_Data()
    {
        var faker = new Faker<GetPatientApiRequest>()
            .RuleFor(o => o.PatientId, f => f.Random.Guid());

        var allData = faker.Generate(5);

        return allData.Select(x => new object[] {x});
    }

    private static IEnumerable<object[]> GetPatient_Fail_EmptyData()
    {
        var faker = new Faker<GetPatientApiRequest>()
            .RuleFor(o => o.PatientId, f => f.Random.Guid());

        //One that doesn't exist for example
        var data = faker.Generate();

        var allData = new List<GetPatientApiRequest>()
        {
            new GetPatientApiRequest(),
            data
        };

        return allData.Select(x => new object[] {x});
    }
    
    public void Dispose()
    {
    }
}