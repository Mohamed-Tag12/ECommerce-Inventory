using Projact__ItI.Models;

namespace Projact__ItI.Context
{
    // Fills the new modules with sample data the first time the app runs.
    // Each part only runs if its table is empty.
    public static class DataSeeder
    {
        private static readonly (string Name, string Email, string Phone, string City)[] CustomerData =
        {
            ("Ahmed Hassan",  "ahmed.hassan@example.com",  "0100 123 4567", "Cairo"),
            ("Sara Ali",      "sara.ali@example.com",      "0111 234 5678", "Giza"),
            ("Mohamed Salah", "m.salah@example.com",       "0122 345 6789", "Alexandria"),
            ("Nour Ibrahim",  "nour.ibrahim@example.com",  "0100 456 7890", "Cairo"),
            ("Omar Khaled",   "omar.khaled@example.com",   "0155 567 8901", "Mansoura"),
            ("Mariam Adel",   "mariam.adel@example.com",   "0106 678 9012", "Tanta"),
            ("Youssef Tarek", "youssef.t@example.com",     "0112 789 0123", "Cairo"),
            ("Hana Mostafa",  "hana.mostafa@example.com",  "0127 890 1234", "Giza"),
            ("Karim Samir",   "karim.samir@example.com",   "0101 901 2345", "Alexandria"),
            ("Laila Fathy",   "laila.fathy@example.com",   "0114 012 3456", "Cairo"),
            ("Mostafa Reda",  "mostafa.reda@example.com",  "0128 123 7890", "Suez"),
            ("Dina Wael",     "dina.wael@example.com",     "0109 234 8901", "Giza"),
        };

        public static void Seed()
        {
            SeedCustomers();
            SeedSuppliers();
            SeedSales();
            SeedPurchases();
            SeedExpenses();
        }

        // ---------------------------------------------------------------- customers
        private static void SeedCustomers()
        {
            try
            {
                using var db = new AppDbContext();
                if (db.Customers.Any()) return;

                var rnd = new Random(11);
                var today = DateTime.Today;

                db.Customers.AddRange(CustomerData.Select(c => new Customer
                {
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone,
                    City = c.City,
                    JoinedAt = today.AddDays(-rnd.Next(10, 240))
                }));

                db.SaveChanges();
            }
            catch { }
        }

        // ---------------------------------------------------------------- suppliers
        private static void SeedSuppliers()
        {
            try
            {
                using var db = new AppDbContext();
                if (db.Suppliers.Any()) return;

                var today = DateTime.Today;

                db.Suppliers.AddRange(new[]
                {
                    new Supplier { Name = "Cairo Tech Distribution", ContactPerson = "Hossam Nabil", Phone = "02 2345 6781", Email = "sales@cairotech.example", City = "Cairo", Supplies = "Laptops, Phones, Headphones", CreatedAt = today.AddDays(-300) },
                    new Supplier { Name = "Delta Home Appliances", ContactPerson = "Ayman Fouad", Phone = "050 223 4455", Email = "orders@deltahome.example", City = "Mansoura", Supplies = "Washers, Microwaves, Refrigerators", CreatedAt = today.AddDays(-260) },
                    new Supplier { Name = "Alexandria Books House", ContactPerson = "Salma Gamal", Phone = "03 4567 8812", Email = "info@alexbooks.example", City = "Alexandria", Supplies = "Novels, Textbooks, Programming", CreatedAt = today.AddDays(-210) },
                    new Supplier { Name = "Nile Fashion Group", ContactPerson = "Rania Said", Phone = "02 3456 7720", Email = "wholesale@nilefashion.example", City = "6th of October", Supplies = "Clothes, Evening Wear", CreatedAt = today.AddDays(-190) },
                    new Supplier { Name = "Sporty Egypt", ContactPerson = "Tamer Lotfy", Phone = "02 3567 9910", Email = "b2b@sportyegypt.example", City = "Giza", Supplies = "Balls, Rackets, Toys, Puzzles", CreatedAt = today.AddDays(-150) },
                    new Supplier { Name = "FreshBean Trading", ContactPerson = "Mona Yehia", Phone = "015 3344 556", Email = "hello@freshbean.example", City = "10th of Ramadan", Supplies = "Coffee, Chocolate, Supplements", CreatedAt = today.AddDays(-120) },
                    new Supplier { Name = "AutoParts Hub", ContactPerson = "Sherif Anwar", Phone = "02 2678 4433", Email = "parts@autohub.example", City = "Cairo", Supplies = "Batteries, Tires", CreatedAt = today.AddDays(-90), IsActive = true },
                });

                db.SaveChanges();
            }
            catch { }
        }

        // ---------------------------------------------------------------- sales (last 30 days)
        private static void SeedSales()
        {
            try
            {
                using var db = new AppDbContext();
                if (db.Sales.Any()) return;

                var products = db.Products.Where(p => p.Quantity > 0).OrderBy(p => p.ProductId).ToList();
                if (products.Count == 0) return;

                var rnd = new Random(2026);

                var names = CustomerData.Select(c => c.Name).ToList();
                names.Add("Walk-in Customer");
                names.Add("Walk-in Customer");

                var sales = new List<Sale>();
                var today = DateTime.Today;

                for (int daysAgo = 29; daysAgo >= 0; daysAgo--)
                {
                    var day = today.AddDays(-daysAgo);
                    int orders = rnd.Next(2, 6);

                    if (day.DayOfWeek == DayOfWeek.Friday || day.DayOfWeek == DayOfWeek.Saturday)
                    {
                        orders += 2;
                    }

                    for (int i = 0; i < orders; i++)
                    {
                        int index = (int)(Math.Pow(rnd.NextDouble(), 1.7) * products.Count);
                        var product = products[Math.Min(index, products.Count - 1)];

                        int maxQty = product.Price > 5000 ? 1 : product.Price > 1000 ? 2 : 4;

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
                            CustomerName = names[rnd.Next(names.Count)],
                            Quantity = rnd.Next(1, maxQty + 1),
                            UnitPrice = product.Price,
                            SaleDate = date
                        });
                    }
                }

                db.Sales.AddRange(sales);
                db.SaveChanges();
            }
            catch { }
        }

        // ---------------------------------------------------------------- purchases (restocks, last 45 days)
        private static void SeedPurchases()
        {
            try
            {
                using var db = new AppDbContext();
                if (db.Purchases.Any()) return;

                var suppliers = db.Suppliers.OrderBy(s => s.SupplierId).ToList();
                var products = db.Products.OrderBy(p => p.ProductId).ToList();
                if (suppliers.Count == 0 || products.Count == 0) return;

                // which product categories each supplier delivers (by seed category ids)
                var map = new Dictionary<string, int[]>
                {
                    ["Cairo Tech Distribution"] = new[] { 1, 11 },
                    ["Delta Home Appliances"] = new[] { 4 },
                    ["Alexandria Books House"] = new[] { 2 },
                    ["Nile Fashion Group"] = new[] { 3, 12 },
                    ["Sporty Egypt"] = new[] { 5, 6 },
                    ["FreshBean Trading"] = new[] { 10, 9 },
                    ["AutoParts Hub"] = new[] { 8 },
                };

                var rnd = new Random(77);
                var today = DateTime.Today;
                var list = new List<Purchase>();

                foreach (var supplier in suppliers)
                {
                    if (!map.TryGetValue(supplier.Name, out var cats)) continue;

                    var pool = products.Where(p => cats.Contains(p.CategoryId)).ToList();
                    if (pool.Count == 0) continue;

                    int count = rnd.Next(3, 6);
                    for (int i = 0; i < count; i++)
                    {
                        var product = pool[rnd.Next(pool.Count)];
                        decimal cost = Math.Round(product.Price * (decimal)(0.58 + rnd.NextDouble() * 0.17) / 5m) * 5m;
                        if (cost < 1) cost = 1;

                        int maxQty = product.Price > 5000 ? 5 : product.Price > 1000 ? 12 : 40;

                        list.Add(new Purchase
                        {
                            SupplierId = supplier.SupplierId,
                            ProductId = product.ProductId,
                            Quantity = rnd.Next(2, maxQty + 1),
                            UnitCost = cost,
                            PurchaseDate = today.AddDays(-rnd.Next(1, 45)).AddHours(rnd.Next(9, 18))
                        });
                    }
                }

                db.Purchases.AddRange(list);
                db.SaveChanges();
            }
            catch { }
        }

        // ---------------------------------------------------------------- expenses (last 3 months)
        private static void SeedExpenses()
        {
            try
            {
                using var db = new AppDbContext();
                if (db.Expenses.Any()) return;

                var rnd = new Random(5);
                var today = DateTime.Today;
                var list = new List<Expense>();

                void Add(string title, string cat, decimal amount, DateTime date, string? note = null)
                {
                    if (date.Date > today) return;
                    list.Add(new Expense { Title = title, Category = cat, Amount = amount, ExpenseDate = date, Note = note });
                }

                for (int m = 2; m >= 0; m--)
                {
                    var first = new DateTime(today.Year, today.Month, 1).AddMonths(-m);

                    Add("Shop rent", "Rent", 18000, first, "Monthly rent");
                    Add("Staff salaries", "Salaries", 52000 + rnd.Next(0, 4) * 1000, first.AddDays(26), "5 employees");
                    Add("Electricity bill", "Utilities", 2800 + rnd.Next(0, 12) * 100, first.AddDays(9));
                    Add("Internet and phone", "Utilities", 950, first.AddDays(4));
                    Add("Social media ads", "Marketing", 3500 + rnd.Next(0, 6) * 250, first.AddDays(6), "Facebook and Instagram campaign");
                    Add("Influencer promotion", "Marketing", 2500 + rnd.Next(0, 5) * 500, first.AddDays(17));
                    Add("Shipping - courier", "Shipping", 600 + rnd.Next(0, 8) * 50, first.AddDays(8));
                    Add("Shipping - courier", "Shipping", 700 + rnd.Next(0, 8) * 50, first.AddDays(19));
                    Add("Shop maintenance", "Maintenance", 900 + rnd.Next(0, 10) * 100, first.AddDays(13), "AC and lighting");
                    Add("Packaging materials", "Other", 750 + rnd.Next(0, 6) * 50, first.AddDays(21));
                }

                db.Expenses.AddRange(list);
                db.SaveChanges();
            }
            catch { }
        }
    }
}
