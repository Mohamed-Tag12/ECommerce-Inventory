using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projact__ItI.Context;

namespace Projact__ItI.Controllers
{
    public class AIController : Controller
    {
        AppDbContext DB = new AppDbContext();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Ask(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return Json("⚠️ Please enter a question.");

            question = question.Trim().ToLower();

            // LOW STOCK
            if (question.Contains("low stock"))
            {
                var products = DB.Products
                    .Where(p => p.Quantity < 10)
                    .OrderBy(p => p.Quantity)
                    .ToList();

                if (!products.Any())
                    return Json("✅ No low stock products.");

                string answer = "<h5>⚠️ Low Stock Products</h5><hr>";

                foreach (var item in products)
                {
                    answer += $"📦 <b>{item.Title}</b> - {item.Quantity} Units<br>";
                }

                return Json(answer);
            }

            // TOTAL PRODUCTS
            if (question.Contains("total products"))
            {
                return Json($"📦 Total Products: <b>{DB.Products.Count()}</b>");
            }

            // MOST EXPENSIVE
            if (question.Contains("most expensive"))
            {
                var product = DB.Products
                    .OrderByDescending(p => p.Price)
                    .FirstOrDefault();

                if (product == null)
                    return Json("No products found.");

                return Json(
                    $"💰 Most Expensive Product<br><br><b>{product.Title}</b><br>{product.Price:N0} EGP"
                );
            }

            // CHEAPEST PRODUCT
            if (question.Contains("cheapest"))
            {
                var product = DB.Products
                    .OrderBy(p => p.Price)
                    .FirstOrDefault();

                if (product == null)
                    return Json("No products found.");

                return Json(
                    $"🏷️ Cheapest Product<br><br><b>{product.Title}</b><br>{product.Price:N0} EGP"
                );
            }

            // OUT OF STOCK
            if (question.Contains("out of stock"))
            {
                var products = DB.Products
                    .Where(p => p.Quantity == 0)
                    .ToList();

                if (!products.Any())
                    return Json("✅ No products are out of stock.");

                string answer = "<h5>❌ Out Of Stock Products</h5><hr>";

                foreach (var item in products)
                {
                    answer += $"📦 {item.Title}<br>";
                }

                return Json(answer);
            }

            // HIGHEST QUANTITY
            if (question.Contains("highest quantity"))
            {
                var product = DB.Products
                    .OrderByDescending(p => p.Quantity)
                    .FirstOrDefault();

                if (product == null)
                    return Json("No products found.");

                return Json(
                    $"📈 Highest Quantity Product<br><br><b>{product.Title}</b><br>{product.Quantity} Units"
                );
            }

            // AVERAGE PRICE
            if (question.Contains("average price"))
            {
                var avg = DB.Products.Average(p => p.Price);

                return Json(
                    $"📊 Average Product Price<br><br><b>{avg:N2} EGP</b>"
                );
            }

            // INVENTORY VALUE
            if (question.Contains("inventory value"))
            {
                var total = DB.Products.Sum(p => p.Price * p.Quantity);

                return Json(
                    $"🏦 Total Inventory Value<br><br><b>{total:N0} EGP</b>"
                );
            }

            // TOTAL CATEGORIES
            if (question.Contains("total categories"))
            {
                return Json(
                    $"🏷️ Total Categories: <b>{DB.Categories.Count()}</b>"
                );
            }

            // STOCK COUNT
            if (question.Contains("stock count"))
            {
                var totalStock = DB.Products.Sum(p => p.Quantity);

                return Json(
                    $"📦 Total Units In Inventory<br><br><b>{totalStock}</b>"
                );
            }

            // PRODUCTS BY CATEGORY
            if (question.Contains("products by category"))
            {
                var categories = DB.Categories
                    .Include(c => c.Products)
                    .ToList();

                string answer = "<h5>📊 Products By Category</h5><hr>";

                foreach (var item in categories)
                {
                    answer += $"🏷️ <b>{item.Name}</b> : {item.Products.Count} Products<br>";
                }

                return Json(answer);
            }

            // TOP PRODUCTS
            if (question.Contains("top products"))
            {
                var products = DB.Products
                    .OrderByDescending(p => p.Price)
                    .Take(5)
                    .ToList();

                string answer = "<h5>⭐ Top 5 Expensive Products</h5><hr>";

                foreach (var item in products)
                {
                    answer += $"📦 {item.Title} - {item.Price:N0} EGP<br>";
                }

                return Json(answer);
            }

            // CHEAP PRODUCTS
            if (question.Contains("cheap products"))
            {
                var products = DB.Products
                    .OrderBy(p => p.Price)
                    .Take(5)
                    .ToList();

                string answer = "<h5>💵 Cheapest 5 Products</h5><hr>";

                foreach (var item in products)
                {
                    answer += $"📦 {item.Title} - {item.Price:N0} EGP<br>";
                }

                return Json(answer);
            }

            // SEARCH PRODUCT
            if (question.StartsWith("product "))
            {
                string productName = question.Replace("product ", "");

                var product = DB.Products
                    .Include(p => p.Category)
                    .FirstOrDefault(p => p.Title.ToLower().Contains(productName));

                if (product == null)
                    return Json("❌ Product not found.");

                return Json($@"
                    <h5>📦 Product Details</h5>
                    <hr>
                    Name: <b>{product.Title}</b><br>
                    Price: <b>{product.Price:N0} EGP</b><br>
                    Quantity: <b>{product.Quantity}</b><br>
                    Category: <b>{product.Category?.Name}</b><br>
                    Description: {product.Description}
                ");
            }

            // SEARCH CATEGORY
            if (question.StartsWith("category "))
            {
                string categoryName = question.Replace("category ", "");

                var category = DB.Categories
                    .Include(c => c.Products)
                    .FirstOrDefault(c => c.Name.ToLower().Contains(categoryName));

                if (category == null)
                    return Json("❌ Category not found.");

                return Json($@"
                    <h5>🏷️ Category Details</h5>
                    <hr>
                    Name: <b>{category.Name}</b><br>
                    Products Count: <b>{category.Products.Count}</b>
                ");
            }

            // CATEGORY LIST
            if (question.Contains("categories"))
            {
                var categories = DB.Categories.ToList();

                string answer = "<h5>🏷️ Categories List</h5><hr>";

                foreach (var item in categories)
                {
                    answer += $"• {item.Name}<br>";
                }

                return Json(answer);
            }

            // BIGGEST CATEGORY
            if (question.Contains("biggest category"))
            {
                var category = DB.Categories
                    .Include(c => c.Products)
                    .OrderByDescending(c => c.Products.Count)
                    .FirstOrDefault();

                if (category == null)
                    return Json("No categories found.");

                return Json(
                    $"🏆 Biggest Category<br><br><b>{category.Name}</b><br>{category.Products.Count} Products"
                );
            }

            return Json(@"
                🤖 <b>Available Commands</b>
                <hr>
                📦 low stock
                <br>📦 total products
                <br>💰 most expensive
                <br>🏷️ cheapest product
                <br>❌ out of stock
                <br>📈 highest quantity
                <br>📊 average price
                <br>🏦 inventory value
                <br>🏷️ total categories
                <br>📦 stock count
                <br>📊 products by category
                <br>⭐ top products
                <br>💵 cheap products
                <br>🏷️ categories
                <br>🏆 biggest category
                <br>🔍 product laptop
                <br>🔍 category electronics
            ");
        }
    }
}