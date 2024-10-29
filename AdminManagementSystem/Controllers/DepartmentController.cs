using AdminManagementSystem.BussinessLogic;
using AdminManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {

        private DepartmentBussinessLogic DepartmentLogic = new DepartmentBussinessLogic();

        // Make First Department Information As Defualt When Open Department Section
        public IActionResult Index()
        {
            var Department = DepartmentLogic.getFirstDepartment();

			ViewBag.DepartmentInformation = Department;
            ViewBag.CountOfMaleStudent = Department.Students_ref.Where(x => x.gender == 'M').Count();
            ViewBag.CountOfFemaleStudent = Department.Students_ref.Where(x => x.gender == 'F').Count();

            return View(DepartmentLogic.getAllDepartment());
        }

        public IActionResult getStudentAtDepartment(int DeptId)
        {
            return PartialView(DepartmentLogic.getStudentsAtDepartment(DeptId));
        }

        public IActionResult getCoursesAtDepartment(int DeptId)
        {
            return PartialView(DepartmentLogic.getCoursesAtDepartment(DeptId));
        }

        public IActionResult InformationAboutDepartment(int DeptId)
        {
            return PartialView(DepartmentLogic.getInformationAboutDepartment(DeptId));
        }

        public IActionResult getFirstStudentAtDepartment(int deptId)
        {
            return PartialView(DepartmentLogic.getFirstStudentAtDepartment(deptId));
        }

        public IActionResult getStudentMarkAtDepartment (int deptId)
        {
            return PartialView(DepartmentLogic.getStudentMarkAtDepartment(deptId));    
        }
    }
}
