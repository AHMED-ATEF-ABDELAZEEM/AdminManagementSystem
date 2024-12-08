using AdminManagementSystem.Models;

namespace AdminManagementSystem.ViewModel
{
    public class CourseInformationVM
    {
        public Course Course { get; set; }

        public int NumberOfDepartment { get; set; }
        public int NumberOfStudent { get; set; }
        public int NumberOfMaleStudent { get; set; }
        public int NumberOfFemaleStudent { get; set; }
    }
}
