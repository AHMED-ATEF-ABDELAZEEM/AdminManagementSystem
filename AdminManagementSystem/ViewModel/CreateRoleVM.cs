using System.ComponentModel.DataAnnotations;

namespace AdminManagementSystem.ViewModel
{
    public class CreateRoleVM
    {
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string RoleName { get; set; }
    }
}
