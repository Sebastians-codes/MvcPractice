using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bulky.Data.Repository.IRepository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null);
    Task<T> Get(Expression<Func<T, bool>> predicate, string? includeProperties = null);
    Task<EntityEntry<T>> AddAsync(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}