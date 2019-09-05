using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebAspNetId.Models.Id;

namespace WebAspNetId.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(
            UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(roleManager.Roles.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return View();
            }
            else
            {
                var result = await roleManager.CreateAsync(new IdentityRole(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                return View();
            }


        }

        [HttpGet]
        public async Task<IActionResult> AddUserToRoleAsync(string roleName)
        {
            IdentityRole role = await roleManager.FindByNameAsync(roleName);

            if (role != null)
            {
                ViewBag.RoleName = roleName;
                return View(userManager.Users.ToList());
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AddThisUserToRole(string roleName, string userId)
        {
            if (!String.IsNullOrWhiteSpace(roleName) && !String.IsNullOrWhiteSpace(userId))
            {
                IdentityRole role = await roleManager.FindByNameAsync(roleName);
                AppUser appUser = await userManager.FindByIdAsync(userId);

                if (role != null && appUser != null)
                {
                    var result = await userManager.AddToRoleAsync(appUser, role.Name);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("AddUserToRoleAsync", new { roleName });
                    }
                }
            }

            return RedirectToAction("AddUserToRoleAsync", new { roleName });
        }
    }
}