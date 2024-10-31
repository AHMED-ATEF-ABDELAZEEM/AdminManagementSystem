using AdminManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

public class UniqueImageAttribute : ValidationAttribute
{
    public string ErrorMessage { get; set; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string Image = value.ToString();
        AppDbContext context = new AppDbContext();
        var student = context.Students.FirstOrDefault(x => x.Image == Image);

        if (student == null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}










