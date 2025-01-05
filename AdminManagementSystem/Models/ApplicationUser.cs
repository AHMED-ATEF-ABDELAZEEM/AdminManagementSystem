using Microsoft.AspNetCore.Identity;

namespace AdminManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAcountBlocked { get; set; }
    }
}
