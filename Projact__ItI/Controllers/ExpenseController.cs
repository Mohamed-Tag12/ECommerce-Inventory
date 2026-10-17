using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Projact__ItI.Context;
using Projact__ItI.Models;
using Projact__ItI.ViewModels;

namespace Projact__ItI.Controllers
{
    public class ExpenseController : Controller
    {
        AppDbContext DB;

        public ExpenseController()
        {
            DB = new AppDbContext();
        }

        [HttpGet]
        public IActionResult Index(string? month)
        {
            DateTime start;

            if (!DateTime.TryParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out start))
            {
                start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }

            var end = start.AddMonths(1);

            var expenses = DB.Expenses
                .Where(e => e.ExpenseDate >= start && e.ExpenseDate < end)
                .OrderByDescending(e => e.ExpenseDate)
                .ToList();

            var revenue = DB.Sales
                .Where(s => s.SaleDate >= start && s.SaleDate < end)
                .ToList()
                .Sum(s => s.Total);

            var stockSpend = DB.Purchases
                .Where(p => p.PurchaseDate >= start && p.PurchaseDate < end)
                .ToList()
                .Sum(p => p.Total);

            var totalExpenses = expenses.Sum(e => e.Amount);

            ViewBag.Month = start;
            ViewBag.PrevMonth = start.AddMonths(-1).ToString("yyyy-MM");
            ViewBag.NextMonth = start.AddMonths(1).ToString("yyyy-MM");
            ViewBag.IsCurrent = start.Year == DateTime.Today.Year && start.Month == DateTime.Today.Month;
            ViewBag.Revenue = revenue;
            ViewBag.StockSpend = stockSpend;
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.Net = revenue - totalExpenses;

            ViewBag.ByCategory = expenses
                .GroupBy(e => e.Category)
                .Select(g => new ExpenseCategoryTotal { Category = g.Key, Total = g.Sum(x => x.Amount) })
                .OrderByDescending(x => x.Total)
                .ToList();

            return View(expenses);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Heading = "Add Expense";
            return View("Form", new Expense { ExpenseDate = DateTime.Today });
        }

        [HttpPost]
        public IActionResult Add(Expense expense)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Heading = "Add Expense";
                return View("Form", expense);
            }

            DB.Expenses.Add(expense);
            DB.SaveChanges();

            TempData["Success"] = "Expense added.";
            return RedirectToAction("Index", new { month = expense.ExpenseDate.ToString("yyyy-MM") });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var expense = DB.Expenses.FirstOrDefault(e => e.ExpenseId == id);

            if (expense == null)
            {
                return NotFound();
            }

            ViewBag.Heading = "Edit Expense";
            return View("Form", expense);
        }

        [HttpPost]
        public IActionResult Edit(Expense expense)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Heading = "Edit Expense";
                return View("Form", expense);
            }

            var existing = DB.Expenses.FirstOrDefault(e => e.ExpenseId == expense.ExpenseId);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Title = expense.Title;
            existing.Category = expense.Category;
            existing.Amount = expense.Amount;
            existing.ExpenseDate = expense.ExpenseDate;
            existing.Note = expense.Note;
            DB.SaveChanges();

            TempData["Success"] = "Expense updated.";
            return RedirectToAction("Index", new { month = expense.ExpenseDate.ToString("yyyy-MM") });
        }

        [HttpPost]
        public IActionResult Delete(int id, string? month)
        {
            var expense = DB.Expenses.FirstOrDefault(e => e.ExpenseId == id);

            if (expense == null)
            {
                return NotFound();
            }

            DB.Expenses.Remove(expense);
            DB.SaveChanges();

            TempData["Success"] = "Expense deleted.";
            return RedirectToAction("Index", new { month });
        }
    }
}
