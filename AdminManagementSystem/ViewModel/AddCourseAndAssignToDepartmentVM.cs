using AdminManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace AdminManagementSystem.ViewModel
{
    public class AddCourseAndAssignToDepartmentVM
    {
        public Course NewCourse { get; set; }

        // Store Id Of Department That You Want To Assign Course To It
        [Required(ErrorMessage = "You Must Choose At Least One Department")]
        public List<int> AssignToDepartment { get; set; }

        public List<Department>? Departments { get; set; }
    }

}
