namespace AdminManagementSystem.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Instructor { get; set; }
        public int Gpa { get; set; }
        public virtual List<Department_Course>? Department_Course_ref { get; set; }
        public virtual List<Student_Course>? Student_Course_ref { get; set; }

    }
}
