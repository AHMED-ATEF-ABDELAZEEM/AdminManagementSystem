using AdminManagementSystem.Models;
using AdminManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Controllers
{
	[Authorize(Roles = "Super Admin")]
	public class UserController : Controller
    {
		private UserManager<ApplicationUser> UserManager;
		private RoleManager<IdentityRole> RoleManager;
		private AppDbContext context;

		public UserController(AppDbContext context, UserManager<ApplicationUser> UserManager, RoleManager<IdentityRole> roleManager)
		{
			this.context = context;
			this.UserManager = UserManager;
			RoleManager = roleManager;
		}

		public IActionResult getAllUser ()
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
						return RedirectToAction("getAllUser", "User");
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

		public async  Task<IActionResult> BlockAcount (string UserId)
		{
			var user = await UserManager.FindByIdAsync(UserId);
			user.IsAcountBlocked = true;
			await UserManager.UpdateAsync(user);
			return RedirectToAction("getAllUser");
		}
		public async Task<IActionResult> UnBlockAcount(string UserId)
		{
			var user = await UserManager.FindByIdAsync(UserId);
			user.IsAcountBlocked = false;
			await UserManager.UpdateAsync(user);
			return RedirectToAction("getAllUser");
		}

		public async Task<IActionResult> getUserInformation (string UserName)
		{
			var user = await UserManager.FindByNameAsync(UserName);
			string RoleId = context.UserRoles.FirstOrDefault(x => x.UserId == user.Id).RoleId;
			string RoleName = context.Roles.FirstOrDefault(x => x.Id == RoleId).Name;
			return View();
		}
	}
}
