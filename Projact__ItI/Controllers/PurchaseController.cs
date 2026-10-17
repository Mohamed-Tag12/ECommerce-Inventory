using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projact__ItI.Context;
using Projact__ItI.Models;

namespace Projact__ItI.Controllers
{
    public class PurchaseController : Controller
    {
        AppDbContext DB;

        public PurchaseController()
        {
            DB = new AppDbContext();
        }

        [HttpGet]
        public IActionResult Index(string? search)
        {
            var purchases = DB.Purchases
                .Include(p => p.Supplier)
                .Include(p => p.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                purchases = purchases.Where(p =>
                    p.Product!.Title.Contains(search) ||
                    p.Supplier!.Name.Contains(search));
            }

            var list = purchases.OrderByDescending(p => p.PurchaseDate).ToList();

            ViewBag.Search = search;
            ViewBag.TotalSpent = list.Sum(p => p.Total);
            ViewBag.TotalUnits = list.Sum(p => p.Quantity);
            ViewBag.Count = list.Count;

            return View(list);
        }

        [HttpGet]
        public IActionResult Add(int? supplierId, int? productId)
        {
            LoadLists();

            return View(new Purchase
            {
                SupplierId = supplierId ?? 0,
                ProductId = productId ?? 0,
                Quantity = 10
            });
        }

        [HttpPost]
        public IActionResult Add([Bind("SupplierId,ProductId,Quantity,UnitCost")] Purchase purchase)
        {
            var product = DB.Products.FirstOrDefault(p => p.ProductId == purchase.ProductId);
            var supplier = DB.Suppliers.FirstOrDefault(s => s.SupplierId == purchase.SupplierId);

            if (product == null)
            {
                ModelState.AddModelError(nameof(Purchase.ProductId), "Please select a valid product.");
            }

            if (supplier == null)
            {
                ModelState.AddModelError(nameof(Purchase.SupplierId), "Please select a valid supplier.");
            }

            if (!ModelState.IsValid)
            {
                LoadLists();
                return View(purchase);
            }

            purchase.PurchaseDate = DateTime.Now;

            // goods received -> stock goes up
            product!.Quantity += purchase.Quantity;

            DB.Purchases.Add(purchase);
            DB.SaveChanges();

            TempData["Success"] = $"Restocked {purchase.Quantity} x {product.Title} from {supplier!.Name}.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var purchase = DB.Purchases.Include(p => p.Product).FirstOrDefault(p => p.PurchaseId == id);

            if (purchase == null)
            {
                return NotFound();
            }

            if (purchase.Product != null)
            {
                if (purchase.Product.Quantity < purchase.Quantity)
                {
                    TempData["Error"] = "Cannot cancel this purchase: part of that stock has already been sold.";
                    return RedirectToAction("Index");
                }

                purchase.Product.Quantity -= purchase.Quantity;
            }

            DB.Purchases.Remove(purchase);
            DB.SaveChanges();

            TempData["Success"] = "Purchase cancelled and stock updated.";
            return RedirectToAction("Index");
        }

        private void LoadLists()
        {
            ViewBag.Suppliers = DB.Suppliers
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToList();

            ViewBag.Products = DB.Products
                .OrderBy(p => p.Title)
                .ToList();
        }
    }
}
