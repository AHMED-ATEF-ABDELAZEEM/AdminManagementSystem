
using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {

        private IDepartmentRepository DepartmentRepository;

        public DepartmentController(IDepartmentRepository DepartmentRepository)
        {
            this.DepartmentRepository = DepartmentRepository;
        }

        // Make First Department Information As Defualt When Open Department Section
        public IActionResult Index()
        {
            var Department = DepartmentRepository.getFirstDepartment();

			ViewBag.DepartmentInformation = Department;
            ViewBag.CountOfMaleStudent = Department.Students_ref.Where(x => x.gender == 'M').Count();
            ViewBag.CountOfFemaleStudent = Department.Students_ref.Where(x => x.gender == 'F').Count();

            return View(DepartmentRepository.getAllDepartment());
        }

        public IActionResult getStudentAtDepartment(int DeptId)
        {
            return PartialView(DepartmentRepository.getStudentsAtDepartment(DeptId));
        }

        public IActionResult getCoursesAtDepartment(int DeptId)
        {
            return PartialView(DepartmentRepository.getCoursesAtDepartment(DeptId));
        }

        public IActionResult InformationAboutDepartment(int DeptId)
        {
            return PartialView(DepartmentRepository.getInformationAboutDepartment(DeptId));
        }

        public IActionResult getFirstStudentAtDepartment(int deptId)
        {
            return PartialView(DepartmentRepository.getFirstStudentAtDepartment(deptId));
        }

        public IActionResult getStudentMarkAtDepartment (int deptId)
        {
            return PartialView(DepartmentRepository.getStudentMarkAtDepartment(deptId));    
        }
    }
}
