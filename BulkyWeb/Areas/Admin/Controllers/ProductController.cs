using Bulky.Data.Repository.IRepository;
using Bulky.Domain.Models;
using Bulky.Domain.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : Controller
{
    public async Task<IActionResult> Index()
    {
        var products = await unitOfWork.Product.GetAllAsync(includeProperties: "Category");

        return View(products);
    }

    public async Task<IActionResult> Upsert(int? id)
    {
        var categories = await unitOfWork.Category.GetAllAsync();

        if (id is null or 0)
        {
            var product = new ProductVM
            {
                Product = new Product(),
                Categories = categories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(product);
        }

        var oldProduct = new ProductVM
        {
            Product = await unitOfWork.Product.Get(x => x.Id == id),
            Categories = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
        };

        return View(oldProduct);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(ProductVM productVm, IFormFile? file)
    {
        if (char.IsLower(productVm.Product.Title[0]))
            productVm.Product.Title = $"{char.ToUpper(productVm.Product.Title[0])}{productVm.Product.Title[1..]}";

        if (productVm.Product.ListPrice is > 1000 or < 1)
            ModelState.AddModelError(nameof(productVm.Product.ListPrice), "List Price must be between 1 and 1000");

        if (!ModelState.IsValid) return View();

        if (file is not null)
            await HandleProductImage(productVm, file);

        if (productVm.Product.Id == 0)
            await unitOfWork.Product.AddAsync(productVm.Product);
        else
            unitOfWork.Product.Update(productVm.Product);

        await unitOfWork.SaveAsync();
        TempData["success"] = "Product created successfully";

        return RedirectToAction("Index");
    }

    private async Task HandleProductImage(ProductVM productVm, IFormFile file)
    {
        var wwwRootPath = webHostEnvironment.WebRootPath;
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var productPath = Path.Combine(wwwRootPath, @"images\product");

        if (!string.IsNullOrWhiteSpace(productVm.Product.ImageUrl))
        {
            var oldImagePath = Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
                System.IO.File.Delete(oldImagePath);
        }

        await using var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create);
        await file.CopyToAsync(fileStream);

        productVm.Product.ImageUrl = @"\images\product\" + fileName;
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