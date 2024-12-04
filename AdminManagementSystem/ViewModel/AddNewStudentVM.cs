using AdminManagementSystem.Models;

namespace AdminManagementSystem.ViewModel
{
    public class AddNewStudentVM
    {
        public Student Student { get; set; }
        public List<Department>? Departments { get; set; }
    }
}
