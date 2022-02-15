using System.Threading.Tasks;
using Bogus;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Storage.Sql;
using Moq;
using Xunit;

namespace Health.Patient.Domain.UnitTests.CreatePatientCommand;

public class CreatePatientAsyncCommandHandlerTests
{
    private readonly CreatePatientAsyncCommandHandler _controller;
    private readonly Faker _faker;
    private readonly Mock<IPatientUnitOfWork> _unitOfWork;

    public CreatePatientAsyncCommandHandlerTests()
    {
        _faker = new Faker();
        _unitOfWork = new Mock<IPatientUnitOfWork>();
        _controller = new CreatePatientAsyncCommandHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task CreatePatientAsyncCommandHandler_Success()
    {
        //Arrange
        var person = new Storage.Sql.Core.Databases.PatientDb.Models.Patient(_faker.Random.Guid(),
            _faker.Person.FirstName,
            _faker.Person.LastName, _faker.Person.DateOfBirth);

        _unitOfWork
            .Setup(x => x.Patients.Add(It.IsAny<Storage.Sql.Core.Databases.PatientDb.Models.Patient>()))
            .ReturnsAsync(person);
        _unitOfWork
            .Setup(x => x.Complete())
            .ReturnsAsync(1);

        var request = new Commands.CreatePatientCommand.CreatePatientCommand(person.FirstName,
            person.LastName, person.DateOfBirth);

        //Act
        var result = await _controller.Handle(request);

        //Assert
        Assert.Equal(person.Id, result);
    }
}