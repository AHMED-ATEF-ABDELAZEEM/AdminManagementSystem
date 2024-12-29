using AdminManagementSystem.Models;

namespace AdminManagementSystem.ViewModel
{
    public class DepartmentInformationVM
    {
        public Department department { get; set; }
        public int NumberOfCourses { get; set; }
        public int NumberOfStudent { get; set; }
        public int NumberOfMaleStudent { get; set; }
        public int NumberOfFemaleStudent { get; set; }

    }
}
