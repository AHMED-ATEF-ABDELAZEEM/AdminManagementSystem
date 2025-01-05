using AdminManagementSystem.Models;
using AdminManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AdminManagementSystem.Controllers
{
    public class AcountController : Controller
    {
        private UserManager<ApplicationUser> UserManager;
        private SignInManager<ApplicationUser> signInManager;

        public AcountController(UserManager<ApplicationUser> UserManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            this.UserManager = UserManager;
            this.signInManager = signInManager;
        }

        public IActionResult Login ()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LogInUserVM UserVM)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(UserVM.Email);

                if (user != null)
                {
                    var ValidAcount = await UserManager.CheckPasswordAsync(user, UserVM.Password);
                    if (ValidAcount)
                    {
                        if (user.IsAcountBlocked)
                        {
                            ModelState.AddModelError("", "Sorry,Your Acount Is Blocked");
                            return View();
                        }
                        // Create Cookie
                        await signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Password Is Not Correct");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Email Is Not Correct");
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout ()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("LogIn");
        }


    }


}

