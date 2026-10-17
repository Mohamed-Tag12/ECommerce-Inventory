using Projact__ItI.Models;

namespace Projact__ItI.Context
{
  
    public static class SalesSeeder
    {
        public static void Seed()
        {
            try
            {
                using var db = new AppDbContext();

                if (db.Sales.Any())
                {
                    return;
                }

                var products = db.Products
                    .Where(p => p.Quantity > 0)
                    .OrderBy(p => p.ProductId)
                    .ToList();

                if (products.Count == 0)
                {
                    return;
                }

                var rnd = new Random(2026);

                var customers = new[]
                {
                    "Ahmed Hassan", "Sara Ali", "Mohamed Salah", "Nour Ibrahim", "Omar Khaled",
                    "Mariam Adel", "Youssef Tarek", "Walk-in Customer", "Hana Mostafa", "Karim Samir",
                    "Walk-in Customer", "Laila Fathy", "Mostafa Reda", "Dina Wael"
                };

                var sales = new List<Sale>();
                var today = DateTime.Today;

                for (int daysAgo = 29; daysAgo >= 0; daysAgo--)
                {
                    int orders = rnd.Next(2, 6);

                    var day = today.AddDays(-daysAgo);
                    if (day.DayOfWeek == DayOfWeek.Friday || day.DayOfWeek == DayOfWeek.Saturday)
                    {
                        orders += 2;
                    }

                    for (int i = 0; i < orders; i++)
                    {
                      
                        int index = (int)(Math.Pow(rnd.NextDouble(), 1.7) * products.Count);
                        var product = products[Math.Min(index, products.Count - 1)];

                        int maxQty = product.Price > 5000 ? 1 : product.Price > 1000 ? 2 : 4;
                        int qty = rnd.Next(1, maxQty + 1);

                        DateTime date;
                        if (daysAgo == 0)
                        {
                            int minutesPassed = Math.Max(1, (int)(DateTime.Now - today).TotalMinutes);
                            date = today.AddMinutes(rnd.Next(0, minutesPassed));
                        }
                        else
                        {
                            date = day.AddHours(9 + rnd.Next(0, 12)).AddMinutes(rnd.Next(0, 60));
                        }

                        sales.Add(new Sale
                        {
                            ProductId = product.ProductId,
                            CustomerName = customers[rnd.Next(customers.Length)],
                            Quantity = qty,
                            UnitPrice = product.Price,
                            SaleDate = date
                        });
                    }
                }

                db.Sales.AddRange(sales);
                db.SaveChanges();
            }
            catch
            {
              
            }
        }
    }
}
