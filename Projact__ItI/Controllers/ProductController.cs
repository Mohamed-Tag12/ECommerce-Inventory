using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projact__ItI.Context;
using Projact__ItI.Models;

namespace Projact__ItI.Controllers
{
    public class ProductController : Controller
    {
        AppDbContext DB;
        public ProductController()
        {
            DB = new AppDbContext();
        }

        [HttpGet]
        public IActionResult Index(string? search, int? categoryId, string? stock, string? sort)
        {
            var products = DB.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p =>
                    p.Title.Contains(search) ||
                    (p.Description != null && p.Description.Contains(search)) ||
                    p.Category!.Name.Contains(search));
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            products = stock switch
            {
                "in" => products.Where(p => p.Quantity > 10),
                "low" => products.Where(p => p.Quantity > 0 && p.Quantity <= 10),
                "out" => products.Where(p => p.Quantity == 0),
                _ => products
            };

            products = sort switch
            {
                "newest" => products.OrderByDescending(p => p.ProductId),
                "price_asc" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                "name" => products.OrderBy(p => p.Title),
                "stock" => products.OrderBy(p => p.Quantity),
                _ => products.OrderBy(p => p.ProductId)
            };

            ViewBag.Categories = DB.Categories.OrderBy(c => c.Name).ToList();
            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.Stock = stock;
            ViewBag.Sort = sort;

            return View(products.ToList());
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = DB.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = DB.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product product, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = DB.Categories.ToList();
                return View(product);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                product.ImagePath = "/images/" + fileName;
            }

            DB.Products.Add(product);
            DB.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = DB.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = DB.Categories.ToList();

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = DB.Categories.ToList();
                return View(product);
            }

            var existingProduct = DB.Products.AsNoTracking()
                .FirstOrDefault(p => p.ProductId == product.ProductId);

            if (existingProduct == null)
            {
                return NotFound();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                // رفع صورة جديدة: نحفظها ونحدّث المسار
                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                product.ImagePath = "/images/" + fileName;
            }
            else
            {
                // مفيش صورة جديدة: نحافظ على القديمة
                product.ImagePath = existingProduct.ImagePath;
            }

            DB.Products.Update(product);
            DB.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = DB.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = DB.Products
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            if (DB.Sales.Any(sl => sl.ProductId == id))
            {
                TempData["Error"] = "This product has sales records, so it cannot be deleted. Set its quantity to 0 instead.";
                return RedirectToAction("Delete", new { id });
            }

            DB.Products.Remove(product);
            DB.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}