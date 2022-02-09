using Health.Patient.Storage.Sql.Core.Repository;
using Health.Patient.Storage.Sql.Core.Repository.PatientDb;

namespace Health.Patient.Storage.Sql;

public interface IPatientUnitOfWork : IDisposable
{
    IPatientRepository Patients { get; }
    Task<int> Complete();
}