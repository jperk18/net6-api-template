using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Health.Patient.Domain.Queries.GetAllPatientsQuery;
using Health.Patient.Storage.Sql;
using Moq;
using Xunit;

namespace Health.Patient.Domain.UnitTests.GetAllPatientsQuery;

public class GetAllPatientsAsyncQueryHandlerTests
{
    private readonly GetAllPatientsAsyncQueryHandler _controller;
    private readonly Faker _faker;
    private readonly Mock<IPatientUnitOfWork> _unitOfWork;
    
    public GetAllPatientsAsyncQueryHandlerTests()
    {
        _faker = new Faker();
        _unitOfWork = new Mock<IPatientUnitOfWork>();
        _controller = new GetAllPatientsAsyncQueryHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task CreatePatientAsyncCommandHandler_Success()
    {
        //Arrange
        var personFaker = new Faker<Storage.Sql.Core.Databases.PatientDb.Models.Patient>()
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                .RuleFor(x => x.LastName, f => f.Person.LastName)
                .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth);

        var generateCount = 15;
        var persons = personFaker.Generate(generateCount);

        _unitOfWork
            .Setup(x => x.Patients.GetAll())
            .Returns(persons);
        
        var request = new Queries.GetAllPatientsQuery.GetAllPatientsQuery();

        //Act
        var response = await _controller.Handle(request);

        //Assert
        Assert.Equal(generateCount, response.Count());
    }
    
    [Fact]
    public async Task CreatePatientAsyncCommandHandler_Fail_ThrowsException()
    {
        //Arrange
        var exceptionMessage = _faker.Lorem.Text();
        _unitOfWork
            .Setup(x => x.Patients.GetAll())
            .Throws(new Exception(exceptionMessage));
        
        var request = new Queries.GetAllPatientsQuery.GetAllPatientsQuery();

        //Act
        var exception = await Assert.ThrowsAsync<Exception>(async () => await _controller.Handle(request));

        //Assert
        Assert.Equal(exceptionMessage, exception.Message);
    }
}