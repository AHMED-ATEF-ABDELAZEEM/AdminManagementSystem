using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using AdminManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AdminManagementSystem.Services
{
    public class DepartmentService
    {
        IDepartmentRepository DepartmentRepository;
        public DepartmentService(IDepartmentRepository DepartmentRepository)
        {
            this.DepartmentRepository = DepartmentRepository;
        }

        public int getIdForFirstDepartment ()
        {
            return DepartmentRepository.getFirstDepartment().DepartmentId;
        }
        public DepartmentInformationVM getDepartmentInformation (int deptId)
        {
            var Model = new DepartmentInformationVM ();
            var department = DepartmentRepository.getDepartment (deptId);

            Model.department = department;

            Model.NumberOfCourses = department.Department_Course_ref.Count ();

            Model.NumberOfStudent = department.Students_ref.Count ();

            Model.NumberOfMaleStudent = department.Students_ref.Where(x => x.gender == 'M').Count();

            Model.NumberOfFemaleStudent = Model.NumberOfStudent - Model.NumberOfMaleStudent;
            
            return Model;

        }

        public List<Department> getAllDepartment ()
        {
            return DepartmentRepository.getAllDepartment ();
        }

        public List<Student> getStudentsAtDepartment (int deptId)
        {
            return DepartmentRepository.getStudentsAtDepartment(deptId);
        }

        public List<Course> getCoursesAtDepartment(int DeptId)
        {
           return DepartmentRepository.getCoursesAtDepartment(DeptId);
        }

        public Department getInformationAboutDepartment(int deptId)
        {
            return DepartmentRepository.getInformationAboutDepartment(deptId);
        }

        public Student getFirstStudentAtDepartment(int deptId)
        {
            return DepartmentRepository.getFirstStudentAtDepartment(deptId);
        }

        public List<StudentWithTotalMark> getStudentMarkAtDepartment(int deptId)
        {
            return DepartmentRepository.getStudentMarkAtDepartment (deptId);
        }
    }
}








