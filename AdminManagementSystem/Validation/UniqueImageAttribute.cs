using AdminManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

public class UniqueImageAttribute : ValidationAttribute
{
    public string ErrorMessage { get; set; }
    private AppDbContext context;

    public UniqueImageAttribute(AppDbContext context)
    {
        this.context = context;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string Image = value.ToString();
        
        var student = context.Students.FirstOrDefault(x => x.Image == Image);

        if (student == null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}










