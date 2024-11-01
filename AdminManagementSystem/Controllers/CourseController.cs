using AdminManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Controllers
{
    public class CourseController : Controller
    {
        private AppDbContext context = new AppDbContext();
        public IActionResult Index()
        {
            // Send All Courses To DropDown Menue That Exist In _CourseLayout
            var courses = context.Courses.ToList();
            // Send Information For First Course
            var FirstCourseInformation = context.Courses
                .Include(x => x.Department_Course_ref)
                .Include(x => x.Student_Course_ref).ThenInclude(x => x.Student_ref)
                .First();

            ViewBag.FirstCourseInformation = FirstCourseInformation;

            ViewBag.CountOfMaleStudent = FirstCourseInformation.Student_Course_ref.Where(x => x.Student_ref.gender == 'M').Count();
            ViewBag.CountOfFemaleStudent = FirstCourseInformation.Student_Course_ref.Where(x => x.Student_ref.gender == 'M').Count();

            return View(courses);
        }

        public IActionResult InformationAboutCourse(int CourseId)
        {
            var CourseInformation = context.Courses
                .Include(x => x.Department_Course_ref)
                .Include(x => x.Student_Course_ref).ThenInclude(x => x.Student_ref)
                .FirstOrDefault(x => x.CourseId == CourseId);
            //ViewBag.CountOfMaleStudent = CourseInformation.Student_Course_ref.Where(x => x.Student_ref.gender == 'M').Count();
            //ViewBag.CountOfFemaleStudent = CourseInformation.Student_Course_ref.Where(x => x.Student_ref.gender == 'M').Count();
            return PartialView(CourseInformation);
        }
        public IActionResult getStudentAtCourse(int CourseId)
        {
            var Students = context.Students_Courses.Include(x => x.Student_ref)
                .Where(x => x.Course_Id == CourseId).Select(x => x.Student_ref).ToList();
            return PartialView(Students);
        }
        public IActionResult getDepartmentThatCourseExistInIt(int CourseId)
        {
            var Departments = context.Departments_Courses.Include(x => x.Department_ref)
                .Where(x => x.Course_Id == CourseId).Select(x => x.Department_ref).ToList();
            return PartialView(Departments);
        }
        public IActionResult getFirstStudentAtCourse(int CourseId)
        {
            return PartialView();
        }
        public IActionResult getStudentMarkAtCourse(int CourseId)
        {
            return PartialView();
        }














    }
}
