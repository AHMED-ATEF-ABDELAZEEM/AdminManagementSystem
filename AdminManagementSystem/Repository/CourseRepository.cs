using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;

namespace AdminManagementSystem.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private AppDbContext context;

        public CourseRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Course> getAllCourses(int DeptId)
        {
            return context.Courses.ToList();
        }

        // Enroll Student At Department That New Course Added To It
        public void EnrollStudentToNewCourse (int CourseId,int DeptId)
        {
            var students = context.Students.Where(x => x.DeptId == DeptId).ToList();
            List<Student_Course> Student_Course_List = new List<Student_Course>();
            foreach (var student in students)
            {
                Student_Course student_Course = new Student_Course();
                student_Course.Course_Id = CourseId;
                student_Course.Student_Id = student.StudentId;
                student_Course.Mark = 0;
                Student_Course_List.Add(student_Course);
            }
            context.Students_Courses.AddRange(Student_Course_List);
            context.SaveChanges();
        }
    }
}
