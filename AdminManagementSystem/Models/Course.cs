using System.ComponentModel.DataAnnotations;

namespace AdminManagementSystem.Models
{
    public class Course
    {
        public string CourseId { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string CourseName { get; set; }
        [Required]
        [MinLength(7)]
        [MaxLength(20)]
        public string Instructor { get; set; }
        [Required]
        [Range(1,4)]
        public int Gpa { get; set; }
        public virtual List<Department_Course>? Department_Course_ref { get; set; }
        public virtual List<Student_Course>? Student_Course_ref { get; set; }

    }
}
