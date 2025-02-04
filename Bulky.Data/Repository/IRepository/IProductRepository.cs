using Bulky.Domain.Models;

namespace Bulky.Data.Repository.IRepository;

public interface IProductRepository : IRepository<Product>
{
    void Update(Product product);
}