namespace Projact__ItI.ViewModels
{
    public class TopProductViewModel
    {
        public string Title { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }

    public class DailySalesViewModel
    {
        public string Label { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }

    public class CategorySalesViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }

    public class ReportViewModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
        public int ItemsSold { get; set; }
        public decimal AverageOrder { get; set; }
        public List<DailySalesViewModel> Daily { get; set; } = new();
        public List<CategorySalesViewModel> ByCategory { get; set; } = new();
        public List<TopProductViewModel> TopProducts { get; set; } = new();
    }
}
