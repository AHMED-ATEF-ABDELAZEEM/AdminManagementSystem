using AdminManagementSystem.Models;
using AdminManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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

        [Authorize]
        public async Task<IActionResult> Logout ()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("LogIn");
        }

        [Authorize]
        public async Task<IActionResult> getAcountInformation()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            var role = await UserManager.GetRolesAsync(user);
            var model = new ShowUserInformationVM();
            model.Id = user.Id;
            model.Name = user.UserName;
            model.Email = user.Email;
            model.Type = role.First();
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword (string Id)
        {
            var model = new ChangePasswordVM();
            model.UserId = Id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.UserId);
                var result = await UserManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (result)
                {
                   var change = await UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                   if (change.Succeeded)
                   {
                        return View("ChangePasswordSuccess");
                   }
                   else
                   {
                        foreach (var error in change.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                   }
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "The Password Is Not Correct");
                }
            }
            var newModel = new ChangePasswordVM();
            newModel.UserId = model.UserId;
            return View(newModel);
        }

    }


}

