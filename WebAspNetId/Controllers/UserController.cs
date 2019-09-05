using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAspNetId.Models.Id;

namespace WebAspNetId.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                return View(newUser);
            }

            return View(newUser);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await userManager.FindByEmailAsync(login.Email);
                var result = await signInManager.PasswordSignInAsync(appUser, login.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

            }

            return View(login);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}