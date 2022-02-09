using System.Linq.Expressions;
using Health.Patient.Storage.Sql.Core.Databases.PatientDb;

namespace Health.Patient.Storage.Sql.Core.Repository.Core.Generic;

public class GenericQueryRepository<T> : IGenericQueryRepository<T> where T : class
{
    protected readonly PatientDbContext _context;
    public GenericQueryRepository(PatientDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }
    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }
    public async Task<T?> GetById(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
}