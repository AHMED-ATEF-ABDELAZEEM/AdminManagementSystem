using AdminManagementSystem.Models;

namespace AdminManagementSystem.BussinessLogic
{
    public class DepartmentBussinessLogic
    {
        private AppDbContext _context = new AppDbContext();

        public List<Department> getAllDepartment ()
        {
            return _context.Departments.ToList();
        }
    }
}
