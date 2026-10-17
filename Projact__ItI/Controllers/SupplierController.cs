using Microsoft.AspNetCore.Mvc;
using Projact__ItI.Context;
using Projact__ItI.Models;
using Projact__ItI.ViewModels;

namespace Projact__ItI.Controllers
{
    public class SupplierController : Controller
    {
        AppDbContext DB;

        public SupplierController()
        {
            DB = new AppDbContext();
        }

        [HttpGet]
        public IActionResult Index(string? search)
        {
            var suppliers = DB.Suppliers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                suppliers = suppliers.Where(s =>
                    s.Name.Contains(search) ||
                    (s.ContactPerson != null && s.ContactPerson.Contains(search)) ||
                    (s.City != null && s.City.Contains(search)) ||
                    (s.Supplies != null && s.Supplies.Contains(search)));
            }

            var list = suppliers.OrderBy(s => s.Name).ToList();
            var purchases = DB.Purchases.ToList();

            var rows = list.Select(s =>
            {
                var mine = purchases.Where(p => p.SupplierId == s.SupplierId).ToList();

                return new SupplierRowViewModel
                {
                    Supplier = s,
                    Purchases = mine.Count,
                    Units = mine.Sum(p => p.Quantity),
                    TotalSpent = mine.Sum(p => p.Total),
                    LastPurchase = mine.Any() ? mine.Max(p => p.PurchaseDate) : null
                };
            }).ToList();

            ViewBag.Search = search;
            ViewBag.TotalSuppliers = rows.Count;
            ViewBag.ActiveSuppliers = rows.Count(r => r.Supplier.IsActive);
            ViewBag.TotalSpent = rows.Sum(r => r.TotalSpent);
            ViewBag.TotalUnits = rows.Sum(r => r.Units);

            return View(rows);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Heading = "Add Supplier";
            return View("Form", new Supplier());
        }

        [HttpPost]
        public IActionResult Add(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Heading = "Add Supplier";
                return View("Form", supplier);
            }

            supplier.CreatedAt = DateTime.Now;
            DB.Suppliers.Add(supplier);
            DB.SaveChanges();

            TempData["Success"] = $"Supplier \"{supplier.Name}\" added.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var supplier = DB.Suppliers.FirstOrDefault(s => s.SupplierId == id);

            if (supplier == null)
            {
                return NotFound();
            }

            ViewBag.Heading = "Edit Supplier";
            return View("Form", supplier);
        }

        [HttpPost]
        public IActionResult Edit(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Heading = "Edit Supplier";
                return View("Form", supplier);
            }

            var existing = DB.Suppliers.FirstOrDefault(s => s.SupplierId == supplier.SupplierId);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = supplier.Name;
            existing.ContactPerson = supplier.ContactPerson;
            existing.Phone = supplier.Phone;
            existing.Email = supplier.Email;
            existing.City = supplier.City;
            existing.Supplies = supplier.Supplies;
            existing.IsActive = supplier.IsActive;
            DB.SaveChanges();

            TempData["Success"] = "Supplier updated.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var supplier = DB.Suppliers.FirstOrDefault(s => s.SupplierId == id);

            if (supplier == null)
            {
                return NotFound();
            }

            if (DB.Purchases.Any(p => p.SupplierId == id))
            {
                TempData["Error"] = "This supplier has purchase records, so it cannot be deleted. Mark it as inactive instead.";
                return RedirectToAction("Index");
            }

            DB.Suppliers.Remove(supplier);
            DB.SaveChanges();

            TempData["Success"] = "Supplier deleted.";
            return RedirectToAction("Index");
        }
    }
}
