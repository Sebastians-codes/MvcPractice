using System.Linq.Expressions;
using Bulky.Data.Repository.IRepository;
using Bulky.Domain.Models;
using BulkyWeb.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bulky.Data.Repository;

public class ProductRepository(ApplicationDbContext context) : Repository<Product>(context), IProductRepository
{
    public async void Update(Product product)
    {
        var objFromDb = await context.FindAsync<Product>(product.Id);

        if (objFromDb is null) return;
        objFromDb.Title = product.Title;
        objFromDb.Description = product.Description;
        objFromDb.Price = product.Price;
        objFromDb.Category = product.Category;
        objFromDb.Author = product.Author;
        objFromDb.Price50 = product.Price50;
        objFromDb.Price100 = product.Price100;
        objFromDb.ListPrice = product.ListPrice;
        objFromDb.ISBN = product.ISBN;
        objFromDb.CategoryId = product.CategoryId;
        if (product.ImageUrl is not null)
            objFromDb.ImageUrl = product.ImageUrl;
    }
}