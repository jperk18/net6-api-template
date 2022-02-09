namespace Health.Patient.Storage.Sql.Core.Repository.Core.Generic;

public interface IGenericRepository<T> : IGenericQueryRepository<T> where T : class
{
    Task<T> Add(T entity);
    Task<IEnumerable<T>> AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}