using System.Linq.Expressions;

namespace Health.Patient.Storage.Sql.Core.Repository.Core.Generic;

public interface IGenericQueryRepository<T> where T : class
{
    Task<T?> GetById(Guid id);
    IEnumerable<T> GetAll();
    IEnumerable<T?> Find(Expression<Func<T, bool>> expression);
}