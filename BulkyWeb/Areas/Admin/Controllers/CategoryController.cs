using Bulky.Data.Repository.IRepository;
using Bulky.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController(IUnitOfWork unitOfWork) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await unitOfWork.Category.GetAllAsync() as List<Category>);

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (char.IsLower(category.Name[0]))
            category.Name = $"{char.ToUpper(category.Name[0])}{category.Name[1..]}";

        if (category.DisplayOrder is > 99 or < 1)
            ModelState.AddModelError(nameof(category.DisplayOrder), "Display order must be between 1 and 99");

        if (!ModelState.IsValid) return View();

        await unitOfWork.Category.AddAsync(category);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Category created successfully";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null or 0)
            return NotFound();

        var category = await unitOfWork.Category.Get(x => x.Id == id);

        if (category is null)
            return NotFound();

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category category)
    {
        if (!ModelState.IsValid) return View();

        unitOfWork.Category.Update(category);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Category updated successfully";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null or 0)
            return NotFound();

        var category = await unitOfWork.Category.Get(x => x.Id == id);

        if (category is null)
            return NotFound();

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName(nameof(Delete))]
    public async Task<IActionResult> DeletePost(int? id)
    {
        var category = await unitOfWork.Category.Get(x => x.Id == id);

        if (category is null)
            return NotFound();

        unitOfWork.Category.Remove(category);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction("Index");
    }
}