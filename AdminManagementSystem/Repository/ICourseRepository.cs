using AdminManagementSystem.Models;

namespace AdminManagementSystem.Repository
{
    public interface ICourseRepository
    {
        public List<Course> getAllCourses();

        void SaveEnrollmentStudentToCourse(List<Student_Course> Enrollments);

        List<Course> getAllCoursesAtDepartment(string DeptId);

        Course getCourseUsingId(string CourseId);

        Course getFirstCourse();

        void SaveNewCourse(Course course);

        void RegisterCourseToDepartment(List<Department_Course> Department_Course);
        void RegisterCourseToDepartment(Department_Course DepartmentCourse);

        bool IsCourseExist(string CourseName);




	}
}
