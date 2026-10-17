using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projact__ItI.Context;
using Projact__ItI.Models;
using Projact__ItI.ViewModels;

namespace Projact__ItI.Controllers
{
    public class ReportController : Controller
    {
        AppDbContext DB;

        public ReportController()
        {
            DB = new AppDbContext();
        }

        [HttpGet]
        public IActionResult Index(DateTime? from, DateTime? to)
        {
            var (fromDate, toDate) = NormalizeRange(from, to);
            var sales = LoadSales(fromDate, toDate);

            var model = new ReportViewModel
            {
                From = fromDate,
                To = toDate,
                Revenue = sales.Sum(s => s.Total),
                Orders = sales.Count,
                ItemsSold = sales.Sum(s => s.Quantity)
            };

            model.AverageOrder = model.Orders == 0 ? 0 : model.Revenue / model.Orders;


            var days = (toDate - fromDate).Days + 1;
            model.Daily = Enumerable.Range(0, days)
                .Select(i => fromDate.AddDays(i))
                .Select(d => new DailySalesViewModel
                {
                    Label = d.ToString("dd MMM"),
                    Total = sales.Where(s => s.SaleDate.Date == d).Sum(s => s.Total)
                })
                .ToList();

            model.ByCategory = sales
                .GroupBy(s => s.Product?.Category?.Name ?? "Uncategorized")
                .Select(g => new CategorySalesViewModel
                {
                    Name = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Revenue)
                .ToList();

            model.TopProducts = sales
                .GroupBy(s => s.Product?.Title ?? "Deleted product")
                .Select(g => new TopProductViewModel
                {
                    Title = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(10)
                .ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Export(DateTime? from, DateTime? to)
        {
            var (fromDate, toDate) = NormalizeRange(from, to);
            var sales = LoadSales(fromDate, toDate);

            var sb = new StringBuilder();
            sb.AppendLine("Sale ID,Date,Product,Category,Customer,Quantity,Unit Price,Total");

            foreach (var s in sales)
            {
                sb.AppendLine(string.Join(",",
                    s.SaleId,
                    s.SaleDate.ToString("yyyy-MM-dd HH:mm"),
                    Csv(s.Product?.Title),
                    Csv(s.Product?.Category?.Name),
                    Csv(s.CustomerName),
                    s.Quantity,
                    s.UnitPrice.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture),
                    s.Total.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)));
            }

           
            var bytes = new UTF8Encoding(true).GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
                .ToArray();

            return File(bytes, "text/csv", $"sales-{fromDate:yyyyMMdd}-{toDate:yyyyMMdd}.csv");
        }

        private static (DateTime from, DateTime to) NormalizeRange(DateTime? from, DateTime? to)
        {
            var toDate = (to ?? DateTime.Today).Date;
            var fromDate = (from ?? toDate.AddDays(-29)).Date;

            if (fromDate > toDate)
            {
                (fromDate, toDate) = (toDate, fromDate);
            }

            if ((toDate - fromDate).Days > 365)
            {
                fromDate = toDate.AddDays(-365);
            }

            return (fromDate, toDate);
        }

        private List<Sale> LoadSales(DateTime fromDate, DateTime toDate)
        {
            var end = toDate.AddDays(1);

            return DB.Sales
                .Include(s => s.Product)
                    .ThenInclude(p => p!.Category)
                .Where(s => s.SaleDate >= fromDate && s.SaleDate < end)
                .OrderBy(s => s.SaleDate)
                .ToList();
        }

        private static string Csv(string? value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
