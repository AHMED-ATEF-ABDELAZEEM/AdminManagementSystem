using AdminManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.BussinessLogic
{
    public class Department_Course_BussinessLogic
    {
        private AppDbContext context = new AppDbContext();

        public List<Department_Course> getCoursesAtDepartment (int DeptId)
        {
            return context.Departments_Courses.Include(x => x.Course_ref)
                .Where(x => x.Department_Id == DeptId).ToList();
        }
    }
}
