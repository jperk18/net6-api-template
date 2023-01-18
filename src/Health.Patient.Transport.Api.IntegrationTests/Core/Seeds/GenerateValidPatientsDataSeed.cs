using System.Collections.Generic;
using Bogus;
using Health.Patient.Storage.Sql.Core.Databases.PatientDb;

namespace Health.Patient.Transport.Api.IntegrationTests.Core.Seeds;

public class GenerateValidPatientsDataSeed : IDataSeed
{
    private readonly int _count;

    public GenerateValidPatientsDataSeed(int count = 50)
    {
        _count = count;
    }

    public IEnumerable<Storage.Sql.Core.Databases.PatientDb.Models.Patient>? Patients { get; private set; }
    
    public void Populate(PatientDbContext dbContext)
    {
        var patientsFaker = new Faker<Storage.Sql.Core.Databases.PatientDb.Models.Patient>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.FirstName, f => f.Person.FirstName)
            .RuleFor(x => x.LastName, f => f.Person.LastName)
            .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth);

        Patients = patientsFaker.Generate(_count);
        
        dbContext.Patients.AddRange(Patients);
    }
}