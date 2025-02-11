using System.Linq.Expressions;
using Bulky.Data.Repository.IRepository;
using BulkyWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bulky.Data.Repository;

public class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : class
{
    private readonly DbSet<T>? _dbSet = context?.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null)
    {
        var items = _dbSet.AsQueryable();
        if (!string.IsNullOrWhiteSpace(includeProperties))
            items = includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries).Aggregate(items,
                (current, includeProperty) => current.Include(includeProperty));

        return await items.ToListAsync();
    }

    public async Task<T> Get(Expression<Func<T, bool>> predicate, string? includeProperties = null)
    {
        var items = _dbSet.AsQueryable();
        if (!string.IsNullOrWhiteSpace(includeProperties))
            items = includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries).Aggregate(items,
                (current, includeProperty) => current.Include(includeProperty));

        return await items.FirstOrDefaultAsync(predicate);
    }

    public async Task<EntityEntry<T>> AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);

    public void Remove(T entity) =>
        _dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities) =>
        _dbSet.RemoveRange(entities);
}