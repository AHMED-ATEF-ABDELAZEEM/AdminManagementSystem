using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private AppDbContext context;

        public CourseRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Course> getAllCourses()
        {
            return context.Courses.ToList();
        }

        public List<Course> getAllCoursesAtDepartment (string DeptId)
        {
			var Courses = context.Departments_Courses
            .Where(x => x.Department_Id == DeptId)
            .Select(x => x.Course_ref).ToList();
            return Courses;
		}

		public Course getCourseUsingId(string CourseId)
		{
            return context.Courses
                   .Include(x => x.Department_Course_ref)
                   .Include(x => x.Student_Course_ref).ThenInclude(x => x.Student_ref)
                   .FirstOrDefault(x => x.CourseId == CourseId);
		}

		public Course getFirstCourse()
		{
            return context.Courses.FirstOrDefault();
		}

        public void SaveNewCourse(Course course)
        {
            if (course.CourseId == null)
            {
                course.CourseId = Guid.NewGuid().ToString();
            }
            context.Courses.Add(course);     
            context.SaveChanges();
        }

        public void RegisterCourseToDepartment(List<Department_Course> Department_Course)
        {
            context.Departments_Courses.AddRange(Department_Course);
            context.SaveChanges() ;
        }

        public void SaveEnrollmentStudentToCourse(List<Student_Course> Enrollments)
        {
            context.Students_Courses.AddRange (Enrollments);
            context.SaveChanges() ;
        }

        public void RegisterCourseToDepartment(Department_Course DepartmentCourse)
        {
            context.Departments_Courses .Add(DepartmentCourse);
            context.SaveChanges() ;
        }

        public bool IsCourseExist(string CourseName)
        {
            var course = context.Courses.FirstOrDefault(x => x.CourseName.ToLower() == CourseName.ToLower());

            return course != null;
        }
    }
}
