using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projact__ItI.Context;
using Projact__ItI.Models;

namespace Projact__ItI.Controllers
{
    public class CategoryController : Controller
    {
        AppDbContext DB;

        public CategoryController()
        {
            DB = new AppDbContext();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = DB.Categories .Include(c => c.Products).ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var category = DB.Categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            DB.Categories.Add(category);
            DB.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = DB.Categories .FirstOrDefault(c => c.CategoryId == id);
 
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            DB.Categories.Update(category);
            DB.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = DB.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = DB.Categories
                .FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            DB.Categories.Remove(category);
            DB.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

