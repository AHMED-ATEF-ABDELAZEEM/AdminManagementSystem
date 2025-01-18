﻿
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

            var CountOfDepartment = DepartmentService.getAllDepartment().Count();

            if (CountOfDepartment == 0)
            {
                return RedirectToAction("AddNewDepartment");
            }


			var DepartmentId = DepartmentService.getIdForFirstDepartment();

            var Model = DepartmentService.getDepartmentInformation(DepartmentId);

            // Send All Courses To DropDown Menue That Exist In _CourseLayout

            ViewBag.Departments = DepartmentService.getAllDepartment();

            return View(Model);
        }

        public IActionResult AddNewDepartment ()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult SaveNewDepartment (Department department)
        {
            if (ModelState.IsValid)
            {
                DepartmentService.SaveNewDepartment(department);
                return RedirectToAction("Index");
            }
            return View("AddNewDepartment");
        }

		public IActionResult getStudentAtDepartment(string DeptId)
        {
            var Model = DepartmentService.getStudentsAtDepartment(DeptId);
            return PartialView(Model);
        }

        public IActionResult getCoursesAtDepartment(string DeptId)
        {
            
            return PartialView(DepartmentService.getCoursesAtDepartment(DeptId));
        }

        public IActionResult InformationAboutDepartment(string DeptId)
        {
            return PartialView(DepartmentService.getInformationAboutDepartment(DeptId));
        }

        public IActionResult getFirstStudentAtDepartment(string deptId)
        {
            return PartialView(DepartmentService.getFirstStudentAtDepartment(deptId));
        }

        public IActionResult getStudentMarkAtDepartment (string deptId)
        {
            return PartialView(DepartmentService.getStudentMarkAtDepartment(deptId));    
        }
    }
}
