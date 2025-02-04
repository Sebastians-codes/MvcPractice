using Bulky.Data.Repository.IRepository;
using Bulky.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController(IUnitOfWork unitOfWork) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await unitOfWork.Product.GetAllAsync() as List<Product>);

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (char.IsLower(product.Title[0]))
            product.Title = $"{char.ToUpper(product.Title[0])}{product.Title[1..]}";

        if (product.ListPrice is > 1000 or < 1)
            ModelState.AddModelError(nameof(product.ListPrice), "List Price must be between 1 and 1000");

        if (!ModelState.IsValid) return View();

        await unitOfWork.Product.AddAsync(product);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Product created successfully";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null or 0)
            return NotFound();

        var category = await unitOfWork.Product.Get(x => x.Id == id);

        if (category is null)
            return NotFound();

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product product)
    {
        if (!ModelState.IsValid) return View();

        unitOfWork.Product.Update(product);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Product updated successfully";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null or 0)
            return NotFound();

        var product = await unitOfWork.Product.Get(x => x.Id == id);

        if (product is null)
            return NotFound();

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName(nameof(Delete))]
    public async Task<IActionResult> DeletePost(int? id)
    {
        var product = await unitOfWork.Product.Get(x => x.Id == id);

        if (product is null)
            return NotFound();

        unitOfWork.Product.Remove(product);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Product deleted successfully";
        return RedirectToAction("Index");
    }
}