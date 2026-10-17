using Projact__ItI.Models;

namespace Projact__ItI.ViewModels
{
    public class LowStockViewModel
    {
        public List<Product> Items { get; set; } = new();
        public int Total { get; set; }
    }
}
