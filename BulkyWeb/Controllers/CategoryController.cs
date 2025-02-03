using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Controllers;

public class CategoryController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await context.Categories.ToListAsync());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (category.Name[0].Equals(category.Name[0]))
            category.Name = $"{char.ToUpper(category.Name[0])}{category.Name[1..]}";

        if (context.Categories.Any(c => c.Name == category.Name))
            ModelState.AddModelError(nameof(category.Name), "Category already exists");

        if (category.DisplayOrder is > 99 or < 1)
            ModelState.AddModelError(nameof(category.DisplayOrder), "Display order must be between 1 and 99");

        if (!ModelState.IsValid) return View();

        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}