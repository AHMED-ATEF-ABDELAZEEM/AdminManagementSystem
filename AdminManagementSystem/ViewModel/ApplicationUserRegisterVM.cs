using System.ComponentModel.DataAnnotations;

namespace AdminManagementSystem.ViewModel
{
    public class ApplicationUserRegisterVM
    {
        [Required]
        [MinLength(7)]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string RoleId { get; set; }

        public List<ShowRoleVM>? Roles { get; set; }
    }
}
