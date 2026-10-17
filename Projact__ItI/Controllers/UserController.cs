using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projact__ItI.Context;
using Projact__ItI.Models;
using Projact__ItI.ViewModels;

namespace Projact__ItI.Controllers
{
    public class UserController : Controller
    {
        AppDbContext DB;

        public UserController()
        {
            DB = new AppDbContext();
        }

      

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool emailExists = DB.Users.Any(u => u.Email == model.Email);

            if (emailExists)
            {
                ModelState.AddModelError("Email",
                    "This email is already registered");

                return View(model);
            }

            User user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password
            };

            DB.Users.Add(user);
            DB.SaveChanges();

            return RedirectToAction("Login");
        }

      
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = DB.Users.FirstOrDefault(u =>
                u.Email == model.Email &&
                u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("",
                    "Invalid Email or Password");

                return View(model);
            }

            HttpContext.Session.SetString("UserName", user.FirstName);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}

