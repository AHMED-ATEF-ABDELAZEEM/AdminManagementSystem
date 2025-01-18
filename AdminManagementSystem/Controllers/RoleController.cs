using AdminManagementSystem.Models;
using AdminManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminManagementSystem.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class RoleController : Controller
    {

        private RoleManager<IdentityRole> roleManager;
        private AppDbContext context;


        public RoleController(RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            this.roleManager = roleManager;
            this.context = context;
        }

        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleVM NewRoleVM)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole();
                role.Name = NewRoleVM.RoleName;
                var result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("getAllRoles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View();
        }

        public IActionResult getAllRoles()
        {
            var Roles = context.Roles.Select(x => new ShowRoleVM
            {
                RoleId = x.Id,
                RoleName = x.Name,
                UserCount = context.UserRoles.Where(y => y.RoleId == x.Id).Count(),
            }).ToList();
            
            return View(Roles);
        }
    }
}
