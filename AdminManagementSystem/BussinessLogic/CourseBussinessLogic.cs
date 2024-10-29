using AdminManagementSystem.Models;

namespace AdminManagementSystem.BussinessLogic
{
    public class CourseBussinessLogic
    {
        private AppDbContext context = new AppDbContext();

        public List<Course> getAllCourses(int DeptId)
        {
            return context.Courses.ToList();
        }
    }
}
