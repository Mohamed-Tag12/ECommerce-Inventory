using Microsoft.AspNetCore.Mvc;
using Projact__ItI.Context;
using Microsoft.EntityFrameworkCore;
using Projact__ItI.Models;
using Projact__ItI.ViewModels;
using System.Diagnostics;

namespace Projact__ItI.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext DB = new AppDbContext();

        public IActionResult Index()
        {
            ViewBag.TotalProducts = DB.Products.Count();

            ViewBag.TotalCategories = DB.Categories.Count();

            ViewBag.TotalStock = DB.Products.Sum(p => p.Quantity);

            ViewBag.TotalValue = DB.Products.Sum(p => p.Price * p.Quantity);

            ViewBag.LastProducts = DB.Products
                .OrderByDescending(p => p.ProductId)
                .Take(5)
                .ToList();

            ViewBag.LastCategories = DB.Categories
                .OrderByDescending(c => c.CategoryId)
                .Take(5)
                .ToList();

            // ===== 1. Stock by Category (للـ Chart) =====
            ViewBag.StockByCategory = DB.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    TotalStock = c.Products.Sum(p => (int?)p.Quantity) ?? 0
                })
                .ToList();

            // ===== 2. Low Stock Alerts (أقل من 10 قطع) =====
            ViewBag.LowStockProducts = DB.Products
                .Where(p => p.Quantity < 10)
                .OrderBy(p => p.Quantity)
                .Take(6)
                .ToList();

            // ===== 3. Recent Activity (آخر منتجات + آخر فئات مجمّعة) =====
            var recentProducts = DB.Products
                .OrderByDescending(p => p.ProductId)
                .Take(4)
                .Select(p => new
                {
                    Type = "Product",
                    Title = p.Title,
                    Id = p.ProductId,
                    Icon = "bi-box-seam",
                    Color = "primary"
                });

            var recentCategories = DB.Categories
                .OrderByDescending(c => c.CategoryId)
                .Take(3)
                .Select(c => new
                {
                    Type = "Category",
                    Title = c.Name,
                    Id = c.CategoryId,
                    Icon = "bi-tags",
                    Color = "success"
                });

            ViewBag.RecentActivity = recentProducts
                .ToList()
                .Cast<dynamic>()
                .Concat(recentCategories.ToList().Cast<dynamic>())
                .ToList();

            // ===== 4. Sales =====
            ViewBag.TotalRevenue = DB.Sales
                .Select(s => (decimal?)(s.UnitPrice * s.Quantity))
                .Sum() ?? 0;

            ViewBag.SalesCount = DB.Sales.Count();

            ViewBag.ItemsSold = DB.Sales.Sum(s => (int?)s.Quantity) ?? 0;

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            ViewBag.TodayRevenue = DB.Sales
                .Where(s => s.SaleDate >= today && s.SaleDate < tomorrow)
                .Select(s => (decimal?)(s.UnitPrice * s.Quantity))
                .Sum() ?? 0;

            ViewBag.RecentSales = DB.Sales
                .Include(s => s.Product)
                .OrderByDescending(s => s.SaleDate)
                .Take(5)
                .ToList();

            ViewBag.TopProducts = DB.Sales
                .GroupBy(s => s.Product!.Title)
                .Select(g => new TopProductViewModel
                {
                    Title = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.UnitPrice * x.Quantity)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToList();

            // Last 7 days sales (for the chart)
            var from = today.AddDays(-6);
            var lastWeek = DB.Sales
                .Where(s => s.SaleDate >= from && s.SaleDate < tomorrow)
                .ToList();

            ViewBag.SalesLast7Days = Enumerable.Range(0, 7)
                .Select(i => from.AddDays(i))
                .Select(d => new DailySalesViewModel
                {
                    Label = d.ToString("dd MMM"),
                    Total = lastWeek
                        .Where(s => s.SaleDate.Date == d)
                        .Sum(s => s.UnitPrice * s.Quantity)
                })
                .ToList();

            // ===== 5. Business (customers / suppliers / expenses / restock) =====
            ViewBag.TotalCustomers = DB.Customers.Count();
            ViewBag.TotalSuppliers = DB.Suppliers.Count();

            var since = DateTime.Today.AddDays(-29);

            decimal revenue30 = DB.Sales
                .Where(s => s.SaleDate >= since)
                .ToList()
                .Sum(s => s.Total);

            decimal expenses30 = DB.Expenses
                .Where(e => e.ExpenseDate >= since)
                .Sum(e => (decimal?)e.Amount) ?? 0;

            decimal purchases30 = DB.Purchases
                .Where(p => p.PurchaseDate >= since)
                .ToList()
                .Sum(p => p.Total);

            ViewBag.Expenses30 = expenses30;
            ViewBag.CashFlow30 = revenue30 - expenses30 - purchases30;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

    }
}