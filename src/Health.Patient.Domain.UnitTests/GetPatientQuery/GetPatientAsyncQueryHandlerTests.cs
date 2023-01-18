using System;
using System.Threading.Tasks;
using Bogus;
using Health.Patient.Domain.Core.Exceptions;
using Health.Patient.Domain.Queries.GetPatientQuery;
using Health.Patient.Storage.Sql;
using Moq;
using Xunit;

namespace Health.Patient.Domain.UnitTests.GetPatientQuery;

public class GetPatientAsyncQueryHandlerTests
{
    private readonly GetPatientAsyncQueryHandler _controller;
    private readonly Faker _faker;
    private readonly Mock<IPatientUnitOfWork> _unitOfWork;
    
    public GetPatientAsyncQueryHandlerTests()
    {
        _faker = new Faker();
        _unitOfWork = new Mock<IPatientUnitOfWork>();
        _controller = new GetPatientAsyncQueryHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task CreatePatientAsyncCommandHandler_Success()
    {
        //Arrange
        var person = new Storage.Sql.Core.Databases.PatientDb.Models.Patient(_faker.Random.Guid(),
            _faker.Person.FirstName,
            _faker.Person.LastName, _faker.Person.DateOfBirth);

        _unitOfWork
            .Setup(x => x.Patients.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(person);
        
        var request = new Queries.GetPatientQuery.GetPatientQuery(person.Id);

        //Act
        var result = await _controller.Handle(request);

        //Assert
        Assert.Equal(person.Id, result.Id);
        Assert.Equal(person.FirstName, result.FirstName);
        Assert.Equal(person.LastName, result.LastName);
        Assert.Equal(person.DateOfBirth, result.DateOfBirth);
    }
    
    [Fact]
    public async Task CreatePatientAsyncCommandHandler_Fail_ThrowsDomainException()
    {
        //Arrange
        var person = new Storage.Sql.Core.Databases.PatientDb.Models.Patient(_faker.Random.Guid(),
            _faker.Person.FirstName,
            _faker.Person.LastName, _faker.Person.DateOfBirth);
        var exceptionMessage = "Record does not exist";
        _unitOfWork
            .Setup(x => x.Patients.GetById(It.IsAny<Guid>()))
            .ReturnsAsync((Storage.Sql.Core.Databases.PatientDb.Models.Patient)null!);
        
        var request = new Queries.GetPatientQuery.GetPatientQuery(person.Id);

        //Act
        var exception = await Assert.ThrowsAsync<DomainValidationException>(async () =>await _controller.Handle(request));

        //Assert
        Assert.Equal(exceptionMessage, exception.Message);
    }
    
    [Fact]
    public async Task CreatePatientAsyncCommandHandler_Fail_ThrowsException()
    {
        //Arrange
        var person = new Storage.Sql.Core.Databases.PatientDb.Models.Patient(_faker.Random.Guid(),
            _faker.Person.FirstName,
            _faker.Person.LastName, _faker.Person.DateOfBirth);
        var exceptionMessage = _faker.Lorem.Text();
        _unitOfWork
            .Setup(x => x.Patients.GetById(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception(exceptionMessage));
        
        var request = new Queries.GetPatientQuery.GetPatientQuery(person.Id);

        //Act
        var exception = await Assert.ThrowsAsync<Exception>(async () =>await _controller.Handle(request));

        //Assert
        Assert.Equal(exceptionMessage, exception.Message);
    }
}