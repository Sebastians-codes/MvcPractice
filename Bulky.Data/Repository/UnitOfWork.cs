using Bulky.Data.Repository.IRepository;
using BulkyWeb.Data;

namespace Bulky.Data.Repository;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public ICategoryRepository Category { get; } = new CategoryRepository(context);
    public IProductRepository Product { get; } = new ProductRepository(context);

    public async Task SaveAsync() =>
        await context.SaveChangesAsync();
}