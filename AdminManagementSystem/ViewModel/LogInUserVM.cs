using System.ComponentModel.DataAnnotations;

namespace AdminManagementSystem.ViewModel
{
    public class LogInUserVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
