
using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using AdminManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {

        private DepartmentService DepartmentService;

        public DepartmentController(DepartmentService DepartmentService)
        {
            this.DepartmentService = DepartmentService;
        }

        // Make First Department Information As Defualt When Open Department Section
        public IActionResult Index()
        {

            var DepartmentId = DepartmentService.getIdForFirstDepartment();

            var Model = DepartmentService.getDepartmentInformation(DepartmentId);

            // Send All Courses To DropDown Menue That Exist In _CourseLayout

            ViewBag.Departments = DepartmentService.getAllDepartment();

            return View(Model);
        }

        public IActionResult getStudentAtDepartment(int DeptId)
        {
            var Model = DepartmentService.getStudentsAtDepartment(DeptId);
            return PartialView(Model);
        }

        public IActionResult getCoursesAtDepartment(int DeptId)
        {
            
            return PartialView(DepartmentService.getCoursesAtDepartment(DeptId));
        }

        public IActionResult InformationAboutDepartment(int DeptId)
        {
            return PartialView(DepartmentService.getInformationAboutDepartment(DeptId));
        }

        public IActionResult getFirstStudentAtDepartment(int deptId)
        {
            return PartialView(DepartmentService.getFirstStudentAtDepartment(deptId));
        }

        public IActionResult getStudentMarkAtDepartment (int deptId)
        {
            return PartialView(DepartmentService.getStudentMarkAtDepartment(deptId));    
        }
    }
}
