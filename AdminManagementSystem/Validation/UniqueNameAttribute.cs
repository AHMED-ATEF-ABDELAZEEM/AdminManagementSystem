using AdminManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace AdminManagementSystem.Validation
{
    public class UniqueNameAttribute : ValidationAttribute
    {
        public string ErrorMessage { get; set; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string Name = value.ToString();
            AppDbContext context = new AppDbContext();
            var student = context.Students.FirstOrDefault(x => x.StudentName == Name);

            // add update
            if (student == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }

}










