using System.Linq.Expressions;
using Bulky.Data.Repository.IRepository;
using Bulky.Domain.Models;
using BulkyWeb.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bulky.Data.Repository;

public class CategoryRepository(ApplicationDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public void Update(Category category) =>
        context.Categories.Update(category);
}