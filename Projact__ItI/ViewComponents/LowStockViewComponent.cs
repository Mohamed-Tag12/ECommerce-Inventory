using Microsoft.AspNetCore.Mvc;
using Projact__ItI.Context;
using Projact__ItI.ViewModels;

namespace Projact__ItI.ViewComponents
{
    public class LowStockViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            using var db = new AppDbContext();

            var model = new LowStockViewModel
            {
                Total = db.Products.Count(p => p.Quantity < 10),
                Items = db.Products
                    .Where(p => p.Quantity < 10)
                    .OrderBy(p => p.Quantity)
                    .Take(5)
                    .ToList()
            };

            return View(model);
        }
    }
}
