using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projact__ItI.Context;
using Projact__ItI.Models;
using Projact__ItI.ViewModels;

namespace Projact__ItI.Controllers
{
    public class CustomerController : Controller
    {
        AppDbContext DB;

        public CustomerController()
        {
            DB = new AppDbContext();
        }

        [HttpGet]
        public IActionResult Index(string? search)
        {
            var customers = DB.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                customers = customers.Where(c =>
                    c.Name.Contains(search) ||
                    (c.Email != null && c.Email.Contains(search)) ||
                    (c.Phone != null && c.Phone.Contains(search)) ||
                    (c.City != null && c.City.Contains(search)));
            }

            var list = customers.OrderBy(c => c.Name).ToList();
            var sales = DB.Sales.Where(s => s.CustomerName != null).ToList();

            var rows = list.Select(c =>
            {
                var mine = sales
                    .Where(s => string.Equals(s.CustomerName, c.Name, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return new CustomerRowViewModel
                {
                    Customer = c,
                    Orders = mine.Count,
                    TotalSpent = mine.Sum(s => s.Total),
                    LastPurchase = mine.Any() ? mine.Max(s => s.SaleDate) : null
                };
            }).ToList();

            var monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            ViewBag.Search = search;
            ViewBag.TotalCustomers = rows.Count;
            ViewBag.TotalSpent = rows.Sum(r => r.TotalSpent);
            ViewBag.NewThisMonth = rows.Count(r => r.Customer.JoinedAt >= monthStart);
            ViewBag.Average = rows.Count == 0 ? 0m : rows.Sum(r => r.TotalSpent) / rows.Count;

            return View(rows);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var customer = DB.Customers.FirstOrDefault(c => c.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            var sales = DB.Sales
                .Include(s => s.Product)
                .Where(s => s.CustomerName == customer.Name)
                .OrderByDescending(s => s.SaleDate)
                .ToList();

            var total = sales.Sum(s => s.Total);

            return View(new CustomerDetailsViewModel
            {
                Customer = customer,
                Sales = sales,
                TotalSpent = total,
                AverageOrder = sales.Count == 0 ? 0 : total / sales.Count
            });
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Heading = "Add Customer";
            return View("Form", new Customer());
        }

        [HttpPost]
        public IActionResult Add(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Heading = "Add Customer";
                return View("Form", customer);
            }

            customer.JoinedAt = DateTime.Now;
            DB.Customers.Add(customer);
            DB.SaveChanges();

            TempData["Success"] = $"Customer \"{customer.Name}\" added.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var customer = DB.Customers.FirstOrDefault(c => c.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.Heading = "Edit Customer";
            return View("Form", customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Heading = "Edit Customer";
                return View("Form", customer);
            }

            var existing = DB.Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = customer.Name;
            existing.Email = customer.Email;
            existing.Phone = customer.Phone;
            existing.City = customer.City;
            DB.SaveChanges();

            TempData["Success"] = "Customer updated.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var customer = DB.Customers.FirstOrDefault(c => c.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            DB.Customers.Remove(customer);
            DB.SaveChanges();

            TempData["Success"] = "Customer deleted. Their past sales are kept.";
            return RedirectToAction("Index");
        }
    }
}
