using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAspNetId.Models.Id;

namespace WebAspNetId.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser() { Email = newUser.Email, UserName = newUser.UserName, Age = newUser.Age };
                IdentityResult result = await userManager.CreateAsync(appUser, newUser.Password);

                return RedirectToAction("Index");
            }

            return View(newUser);
        }
    }
}