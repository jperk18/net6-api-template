using Health.Patient.Storage.Sql.Core.Repository.Core.Generic;

namespace Health.Patient.Storage.Sql.Core.Repository.PatientDb;

public interface IPatientRepository : IGenericRepository<Databases.PatientDb.Models.Patient>
{
    
}