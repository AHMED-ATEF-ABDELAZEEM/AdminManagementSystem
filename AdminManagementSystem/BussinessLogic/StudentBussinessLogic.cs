using AdminManagementSystem.Models;

namespace AdminManagementSystem.BussinessLogic
{
    public class StudentBussinessLogic
    {
        private AppDbContext context = new AppDbContext();

        public List<Student> getStudentAtDepartment (int deptId)
        {
            return context.Students.Where(x => x.DeptId == deptId).ToList();
        }

        public List<Student> getAllStudent ()
        {
            return context.Students.ToList();
        }

        public void SaveNewStudent (Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
        }
    }
}
