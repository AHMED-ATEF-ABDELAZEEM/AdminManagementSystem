using AdminManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace AdminManagementSystem.ViewModel
{
	public class ChangePermissionVM
	{
		public string UserId { get; set; }
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? CurrentRole { get; set; }
		public string NewRoleId { get; set; }
		public List<IdentityRole>? Roles { get; set; } 
	}
}
