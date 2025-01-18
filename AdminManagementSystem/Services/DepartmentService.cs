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

        public string getIdForFirstDepartment ()
        {
            return DepartmentRepository.getFirstDepartment().DepartmentId;
        }
        public DepartmentInformationVM getDepartmentInformation (string deptId)
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

        public List<Student> getStudentsAtDepartment (string deptId)
        {
            return DepartmentRepository.getStudentsAtDepartment(deptId);
        }

        public List<Course> getCoursesAtDepartment(string DeptId)
        {
           return DepartmentRepository.getCoursesAtDepartment(DeptId);
        }

        public Department getInformationAboutDepartment(string deptId)
        {
            return DepartmentRepository.getInformationAboutDepartment(deptId);
        }

        public Student getFirstStudentAtDepartment(string deptId)
        {
            return DepartmentRepository.getFirstStudentAtDepartment(deptId);
        }

        public List<StudentWithTotalMark> getStudentMarkAtDepartment(string deptId)
        {
            return DepartmentRepository.getStudentMarkAtDepartment (deptId);
        }

        public void SaveNewDepartment (Department department)
        {
            DepartmentRepository.SaveNewDepartment(department);
        }
    }
}








