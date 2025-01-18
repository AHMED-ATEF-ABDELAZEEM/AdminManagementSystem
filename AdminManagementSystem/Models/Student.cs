using AdminManagementSystem.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminManagementSystem.Models
{
    public class Student
    {
        public string StudentId { get; set; }
        [Required(ErrorMessage = "Student Name Is Required")]
        [MinLength(10,ErrorMessage = "The Name Must Be More Than 9 Char")]
        [MaxLength(20,ErrorMessage = "The Name Must Be Less Than 25 Char")]
        [Display(Name = "Student Name")]

        public string StudentName { get; set; }


        [Range(18,30,ErrorMessage = "Age Must Be Between 18 And 30")]
        public int Age { get; set; }


        [RegularExpression("^[MF]$", ErrorMessage = "Gender must be 'M' or 'F'")]
        public char gender { get; set; }


        [RegularExpression("^(Alex|Cairo|BaniSuef)$", ErrorMessage = "City must be 'Alex' or 'Cairo' or 'BaniSuef'.")]
        public string City { get; set; }

        [RegularExpression(@"^.*\.(png|jpeg|jpg)$", ErrorMessage = "The Image Must Be .png , .jpg or .jpeg")]
        //[UniqueImage(ErrorMessage = "This Image Is Already Use Please Enter Diffrent Image")]
        public string? Image {  get; set; }


        [ForeignKey(nameof(Department_ref))]
        [Display(Name = "Department")]
        public string DeptId { get; set; }

        public virtual Department? Department_ref { get; set; }
        public virtual List<Student_Course>? Student_Course_ref {  get; set; }
    }
}
