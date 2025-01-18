using AdminManagementSystem.Models;
using AdminManagementSystem.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Repository
{
    public interface IDepartmentRepository
    {
        Department getDepartment(string deptId);

        Department getFirstDepartment();

        void SaveNewDepartment(Department department);


		List<Department> getAllDepartment();

        List<Student> getStudentsAtDepartment(string deptId);

        List<Course> getCoursesAtDepartment(string deptId);

        Department getInformationAboutDepartment(string deptId);

        Student getFirstStudentAtDepartment(string deptId);

        List<StudentWithTotalMark> getStudentMarkAtDepartment(string deptId);


        List<Department> getDepartmentThatCourseExistInIt(string CourseId);

    }
}
