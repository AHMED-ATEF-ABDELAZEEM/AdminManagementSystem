using System.ComponentModel.DataAnnotations;

namespace AdminManagementSystem.ViewModel
{
    public class ChangePasswordVM
    {
        public string UserId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

    }
}
