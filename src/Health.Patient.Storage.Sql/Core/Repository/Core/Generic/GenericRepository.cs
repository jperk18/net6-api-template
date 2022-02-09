using Health.Patient.Storage.Sql.Core.Databases.PatientDb;

namespace Health.Patient.Storage.Sql.Core.Repository.Core.Generic;

public class GenericRepository<T> : GenericQueryRepository<T>, IGenericRepository<T> where T : class
{
#pragma warning disable CS0108, CS0114
    private readonly PatientDbContext _context;
#pragma warning restore CS0108, CS0114
    public GenericRepository(PatientDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<T> Add(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<T>> AddRange(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        return entities;
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }
}