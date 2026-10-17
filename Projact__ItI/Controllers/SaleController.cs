using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projact__ItI.Context;
using Projact__ItI.Models;

namespace Projact__ItI.Controllers
{
    public class SaleController : Controller
    {
        AppDbContext DB;

        public SaleController()
        {
            DB = new AppDbContext();
        }

        [HttpGet]
        public IActionResult Index(string? search)
        {
            var sales = DB.Sales
                .Include(s => s.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                sales = sales.Where(s =>
                    s.Product!.Title.Contains(search) ||
                    (s.CustomerName != null && s.CustomerName.Contains(search)));
            }

            var list = sales.OrderByDescending(s => s.SaleDate).ToList();

            ViewBag.TotalRevenue = list.Sum(s => s.UnitPrice * s.Quantity);
            ViewBag.ItemsSold = list.Sum(s => s.Quantity);
            ViewBag.SalesCount = list.Count;
            ViewBag.Search = search;

            return View(list);
        }

        [HttpGet]
        public IActionResult Add(int? productId)
        {
            LoadProducts();

            return View(new Sale
            {
                ProductId = productId ?? 0,
                Quantity = 1
            });
        }

        [HttpPost]
        public IActionResult Add([Bind("ProductId,CustomerName,Quantity")] Sale sale)
        {
            var product = DB.Products.FirstOrDefault(p => p.ProductId == sale.ProductId);

            if (product == null)
            {
                ModelState.AddModelError(nameof(Sale.ProductId), "Please select a valid product.");
            }
            else if (sale.Quantity > product.Quantity)
            {
                ModelState.AddModelError(nameof(Sale.Quantity),
                    $"Only {product.Quantity} item(s) available in stock.");
            }

            if (!ModelState.IsValid)
            {
                LoadProducts();
                return View(sale);
            }

            sale.UnitPrice = product!.Price;
            sale.SaleDate = DateTime.Now;
            sale.CustomerName = string.IsNullOrWhiteSpace(sale.CustomerName)
                ? "Walk-in Customer"
                : sale.CustomerName.Trim();

            product.Quantity -= sale.Quantity;

            DB.Sales.Add(sale);
            DB.SaveChanges();

            TempData["Success"] = $"Sale recorded: {sale.Quantity} x {product.Title}";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Invoice(int id)
        {
            var sale = DB.Sales
                .Include(s => s.Product)
                    .ThenInclude(p => p!.Category)
                .FirstOrDefault(s => s.SaleId == id);

            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var sale = DB.Sales.Include(s => s.Product).FirstOrDefault(s => s.SaleId == id);

            if (sale == null)
            {
                return NotFound();
            }

          
            if (sale.Product != null)
            {
                sale.Product.Quantity += sale.Quantity;
            }

            DB.Sales.Remove(sale);
            DB.SaveChanges();

            TempData["Success"] = "Sale cancelled and stock restored.";

            return RedirectToAction("Index");
        }

        private void LoadProducts()
        {
            ViewBag.Products = DB.Products
                .Where(p => p.Quantity > 0)
                .OrderBy(p => p.Title)
                .ToList();

            ViewBag.Customers = DB.Customers
                .OrderBy(c => c.Name)
                .Select(c => c.Name)
                .ToList();
        }
    }
}
