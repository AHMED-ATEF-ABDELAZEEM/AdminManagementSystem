using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminManagementSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(20)]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }
        [Range(18,30)]
        public int Age { get; set; }
        [RegularExpression("^[MF]$", ErrorMessage = "Gender must be 'M' or 'F'")]
        public char gender { get; set; }
        [RegularExpression("^(Alex|Cairo|BaniSuef)$", ErrorMessage = "City must be 'Alex' or 'Cairo' or 'BaniSuef'.")]
        public string City { get; set; }
        public string? Image {  get; set; }

        [ForeignKey(nameof(Department_ref))]
        [Display(Name = "Department")]
        public int DeptId { get; set; }

        public virtual Department? Department_ref { get; set; }
        public virtual List<Student_Course>? Student_Course_ref {  get; set; }
    }
}
