
using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using AdminManagementSystem.Services;
using AdminManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private CourseService CourseService;

        public CourseController(CourseService CourseService)
        {

            this.CourseService = CourseService;
        }

        // Show Information About First Course
        public IActionResult Index()
        {
            var Courses = CourseService.getAllCourses();

            int CountOfCourses = Courses.Count();

            if (CountOfCourses == 0)
            {
                return RedirectToAction("AddNewCourse");
            }


            string CourseId = CourseService.getIdForFirstCourse();

            var Model = CourseService.getCourseInformation(CourseId);

            // Send All Courses To DropDown Menue That Exist In _CourseLayout
            ViewBag.Courses = Courses;


            return View(Model);
        }

        public IActionResult InformationAboutCourse(string CourseId)
        {
            var Model = CourseService.getCourseInformation(CourseId);

            return PartialView(Model);
        }
        public IActionResult getStudentAtCourse(string CourseId)
        {
            var Students = CourseService.getStudentAtCourse(CourseId);

            return PartialView(Students);
        }
        public IActionResult getDepartmentThatCourseExistInIt(string CourseId)
        {
            var Departments = CourseService.getDepartmentThatCourseExistInIt(CourseId);
            return PartialView(Departments);
        }
        public IActionResult getFirstStudentAtCourse(string CourseId)
        {
            var FirstStudent = CourseService.getFirstStudentAtCourse(CourseId);
            return PartialView(FirstStudent);
        }
        public IActionResult getStudentWithMarkAtCourse(string CourseId)
        {
            //        var StudentMark = context.Students_Courses
            //            .Include(x => x.Student_ref).ThenInclude(x => x.Department_ref)
            //            .Where(x => x.Course_Id == CourseId)
            //            .Select(x => new StudentWithMarkInCourseVM
            //            {
            //	Name = x.Student_ref.StudentName,
            //                gender = x.Student_ref.gender,
            //                Age = x.Student_ref.Age,
            //                Department = x.Student_ref.Department_ref.DepartmentName,
            //                Mark = x.Mark,
            //}).OrderByDescending(x => x.Mark).ToList();
            var Model = CourseService.getAllStudentWithMarkAtCourse(CourseId);
            return PartialView(Model);
        }


        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult AddNewCourse()
        {
            var Model = new AddCourseAndAssignToDepartmentVM();
            Model.Departments = CourseService.GetAllDepartment();
            return View(Model);
        }

		[Authorize(Roles = "Super Admin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult SaveNewCourse(AddCourseAndAssignToDepartmentVM Model)
        {
            // User Doesnt Choose Any Department
            if (Model.AssignToDepartment.Count == 0)
            {
                ModelState.AddModelError("Model.AssignToDepartment", "Please Choose At Least One Department");
            }

            if (CourseService.IsCourseExist(Model.NewCourse.CourseName))
            {
                ModelState.AddModelError("Model.NewCourse.CourseName", "This Course Is Already Exist Please Enter Diffrent Course");
            }

            if (ModelState.IsValid)
            {
                CourseService.SaveCourseAndRegisterToDepartmentAndEnrollmentStudent(Model);
                return RedirectToAction("Index");
            }
            Model.Departments = CourseService.GetAllDepartment();
            return View("AddNewCourse", Model);
        }



		[Authorize(Roles = "Super Admin,Admin")]
		public IActionResult UpdateStudentMarkInCourse(string CourseId)
        {
            var Model = new UpdateMarkAtCourseVM();
            Model.CourseId = CourseId;
            Model.CourseName = CourseService.GetCourseName(CourseId);
            Model.StudentMarks = CourseService.getStudentMarkAtCourse(CourseId);

            return View(Model);
        }

		[Authorize(Roles = "Super Admin,Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult SaveUpdateStudentMarkInCourse(UpdateMarkAtCourseVM Model)
        {
            if (ModelState.IsValid)
            {
                int CountOfUpdatedMark = CourseService.UpdateStudentMark(Model);

                if (CountOfUpdatedMark == 0)
                {
                    return View("NoUpdatedMessage");
                }
                else
                {
                    return View("SuccessMessage", CountOfUpdatedMark);
                }
            }
            Model.CourseName = CourseService.GetCourseName(Model.CourseId);
            return View("UpdateStudentMarkInCourse", Model);
        }
    }
}












