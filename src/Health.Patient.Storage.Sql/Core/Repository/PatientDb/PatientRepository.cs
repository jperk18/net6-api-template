using Health.Patient.Storage.Sql.Core.Databases.PatientDb;
using Health.Patient.Storage.Sql.Core.Repository.Core.Generic;

namespace Health.Patient.Storage.Sql.Core.Repository.PatientDb;

public class PatientRepository : GenericRepository<Databases.PatientDb.Models.Patient>, IPatientRepository
{
#pragma warning disable CS0108, CS0114
    private readonly PatientDbContext _context;
#pragma warning restore CS0108, CS0114

    public PatientRepository(PatientDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
}