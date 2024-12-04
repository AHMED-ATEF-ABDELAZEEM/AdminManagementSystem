using AdminManagementSystem.Models;

namespace AdminManagementSystem.Repository
{
    public interface ICourseRepository
    {
        public List<Course> getAllCourses(int DeptId);

        // Enroll Student At Department That New Course Added To It
        void EnrollStudentToNewCourse(int CourseId, int DeptId);

        List<Course> getAllCoursesAtDepartment(int DeptId);


	}
}
