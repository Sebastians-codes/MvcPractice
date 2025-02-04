using System.Linq.Expressions;
using Bulky.Data.Repository.IRepository;
using Bulky.Domain.Models;
using BulkyWeb.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bulky.Data.Repository;

public class ProductRepository(ApplicationDbContext context) : Repository<Product>(context), IProductRepository
{
    public void Update(Product product) =>
        context.Products.Update(product);
}