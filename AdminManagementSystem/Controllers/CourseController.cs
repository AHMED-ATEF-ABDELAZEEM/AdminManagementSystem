
using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using AdminManagementSystem.Services;
using AdminManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Controllers
{
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

            int CourseId = CourseService.getIdForFirstCourse();

            var Model = CourseService.getCourseInformation(CourseId);

            // Send All Courses To DropDown Menue That Exist In _CourseLayout
            ViewBag.Courses = CourseService.getAllCourses();


            return View(Model);
        }

        public IActionResult InformationAboutCourse(int CourseId)
        {
            var Model = CourseService.getCourseInformation(CourseId);

            return PartialView(Model);
        }
        public IActionResult getStudentAtCourse(int CourseId)
        {
            var Students = CourseService.getStudentAtCourse(CourseId);

            return PartialView(Students);
        }
        public IActionResult getDepartmentThatCourseExistInIt(int CourseId)
        {
            var Departments = CourseService.getDepartmentThatCourseExistInIt(CourseId);
            return PartialView(Departments);
        }
        public IActionResult getFirstStudentAtCourse(int CourseId)
        {
            var FirstStudent = CourseService.getFirstStudentAtCourse(CourseId);
            return PartialView(FirstStudent);
        }
        public IActionResult getStudentWithMarkAtCourse(int CourseId)
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

        public IActionResult AddNewCourse()
        {
            var Model = new AddCourseAndAssignToDepartmentVM();
            Model.Departments = CourseService.GetAllDepartment();
            return View(Model);
        }

        public IActionResult SaveNewCourse(AddCourseAndAssignToDepartmentVM Model)
        {
            // User Doesnt Choose Any Department
            if (Model.AssignToDepartment == null)
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
                RedirectToAction("Index");
            }
            Model.Departments = CourseService.GetAllDepartment();
            return View("AddNewCourse", Model);
        }




        public IActionResult UpdateStudentMarkInCourse(int CourseId)
        {
            var Model = new UpdateMarkAtCourseVM();
            Model.CourseId = CourseId;
            Model.CourseName = CourseService.GetCourseName(CourseId);
            Model.StudentMarks = CourseService.getStudentMarkAtCourse(CourseId);

            return View(Model);
        }

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












