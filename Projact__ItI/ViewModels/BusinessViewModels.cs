using Projact__ItI.Models;

namespace Projact__ItI.ViewModels
{
    public class CustomerRowViewModel
    {
        public Customer Customer { get; set; } = new();
        public int Orders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime? LastPurchase { get; set; }
    }

    public class CustomerDetailsViewModel
    {
        public Customer Customer { get; set; } = new();
        public List<Sale> Sales { get; set; } = new();
        public decimal TotalSpent { get; set; }
        public decimal AverageOrder { get; set; }
    }

    public class SupplierRowViewModel
    {
        public Supplier Supplier { get; set; } = new();
        public int Purchases { get; set; }
        public int Units { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime? LastPurchase { get; set; }
    }

    public class ExpenseCategoryTotal
    {
        public string Category { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}
