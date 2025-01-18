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
        private RoleManager<IdentityRole> RoleManager;
        private SignInManager<ApplicationUser> signInManager;
        private AppDbContext context;

        public AcountController(UserManager<ApplicationUser> UserManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> RoleManager, AppDbContext context)
        {
            this.UserManager = UserManager;
            this.signInManager = signInManager;
            this.context = context;
            this.RoleManager = RoleManager;
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


        [Authorize(Roles = "Super Admin")]
        public IActionResult Register()
        {
            ApplicationUserRegisterVM UserRegister = new ApplicationUserRegisterVM();
            UserRegister.Roles = context.Roles.Select(x => new ShowRoleVM
            {
                RoleId = x.Id,
                RoleName = x.Name,
            }).ToList();
          
            return View(UserRegister);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Register(ApplicationUserRegisterVM NewUserVM)
        {
            if (NewUserVM.RoleId == "0")
            {
                ModelState.AddModelError("RoleId", "Please Choose Type Of User");
            }
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = NewUserVM.Name;
                user.Email = NewUserVM.Email;
                user.PasswordHash = NewUserVM.Password;
                user.IsAcountBlocked = false;
                var result = await UserManager.CreateAsync(user, NewUserVM.Password);

                if (result.Succeeded)
                {
                    //var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == NewUserVM.RoleId);

                    var role = await RoleManager.FindByIdAsync(NewUserVM.RoleId);

                    if (role != null)
                    {
                        // Assign Role To User
                        await UserManager.AddToRoleAsync(user, role.Name);
                        return View("SuccessMessageCreatedAcount");
                    }

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            NewUserVM.Roles = context.Roles.Select(x => new ShowRoleVM
            {
                RoleId = x.Id,
                RoleName = x.Name,
            }).ToList();
            return View(NewUserVM);
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

        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> BlockAcount(string UserId)
        {
            var user = await UserManager.FindByIdAsync(UserId);
            user.IsAcountBlocked = true;
            await UserManager.UpdateAsync(user);
            return RedirectToAction("getAllAcounts");
        }
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UnBlockAcount(string UserId)
        {
            var user = await UserManager.FindByIdAsync(UserId);
            user.IsAcountBlocked = false;
            await UserManager.UpdateAsync(user);
            return RedirectToAction("getAllAcounts");
        }

        [Authorize(Roles = "Super Admin")]
        public IActionResult getAllAcounts()
        {
            var Users = context.Users
                .Select(x => new ShowUserVM
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    UserEmail = x.Email,
                    IsBlocked = x.IsAcountBlocked,
                    UserType = context.UserRoles
                        .Where(ur => ur.UserId == x.Id)
                        .Join(context.Roles,
                            ur => ur.RoleId,
                            r => r.Id,
                            (ur, r) => r.Name) // Get the role name
                        .FirstOrDefault() // Select the first role (if a user has multiple roles, adjust logic as needed)
                })
                .ToList();
            return View(Users);
        }


        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ChangePermission(string UserId)
        {
            var model = new ChangePermissionVM();

            var ApplicationUser = await UserManager.FindByIdAsync(UserId);

            model.UserId = ApplicationUser.Id;
            model.Name = ApplicationUser.UserName;
            model.Email = ApplicationUser.Email;

            var Roles = await UserManager.GetRolesAsync(ApplicationUser);
            var UserRole = Roles[0];
            model.CurrentRole = UserRole;

            model.Roles = context.Roles.Where(x => x.Name != UserRole).ToList();

			return View(model);
        }


		[Authorize(Roles = "Super Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePermission (ChangePermissionVM model)
        {
            if (ModelState.IsValid)
            {
                // Remove User From Current Role
                var User = await UserManager.FindByIdAsync (model.UserId);
                await UserManager.RemoveFromRoleAsync(User, model.CurrentRole);
                // Add User To New Role
                var NewRole = await RoleManager.FindByIdAsync(model.NewRoleId);
                await UserManager.AddToRoleAsync(User,NewRole.Name);
                return RedirectToAction("getAllAcounts");              
            }
            return View(model);
        }

        public async  Task<IActionResult> DeleteAcount (string UserId)
        {
            var User = await UserManager.FindByIdAsync(UserId);
            await UserManager.DeleteAsync(User);
            return RedirectToAction("getAllAcounts");
        }


	}


}

