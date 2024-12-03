using AdminManagementSystem.Models;
using AdminManagementSystem.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Repository
{
    public interface IDepartmentRepository
    {
        Department getDepartment(int deptId);

        Department getFirstDepartment();

        List<Department> getAllDepartment();

        List<Student> getStudentsAtDepartment(int deptId);

        List<Course> getCoursesAtDepartment(int deptId);

        Department getInformationAboutDepartment(int deptId);

        Student getFirstStudentAtDepartment(int deptId);

        List<StudentWithTotalMark> getStudentMarkAtDepartment(int deptId);


    }
}
