using AdminManagementSystem.BussinessLogic;
using AdminManagementSystem.Models;
using AdminManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Controllers
{
	public class CourseController : Controller
    {
        private AppDbContext context = new AppDbContext();
        private CourseBussinessLogic CourseLogic = new CourseBussinessLogic();

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
            ViewBag.CountOfFemaleStudent = FirstCourseInformation.Student_Course_ref.Where(x => x.Student_ref.gender == 'F').Count();

            return View(courses);
        }

        public IActionResult InformationAboutCourse(int CourseId)
        {
            var Course = context.Courses
                .Include(x => x.Department_Course_ref)
                .Include(x => x.Student_Course_ref).ThenInclude(x => x.Student_ref)
                .FirstOrDefault(x => x.CourseId == CourseId);
            //ViewBag.CountOfMaleStudent = CourseInformation.Student_Course_ref.Where(x => x.Student_ref.gender == 'M').Count();
            //ViewBag.CountOfFemaleStudent = CourseInformation.Student_Course_ref.Where(x => x.Student_ref.gender == 'M').Count();
            return PartialView(Course);
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
            var FirstStudent = context.Students_Courses
                .Include(x => x.Student_ref)
                .Where(x => x.Course_Id == CourseId)
                .OrderByDescending(x => x.Mark)
                .Select(x => new FirstStudentWithMarkVM
                {
					Name = x.Student_ref.StudentName,
					gender = x.Student_ref.gender == 'M' ? "Male" : "Female",
					Age = x.Student_ref.Age,
					Department = x.Student_ref.Department_ref.DepartmentName,
                    Image = x.Student_ref.Image != null ? x.Student_ref.Image : x.Student_ref.gender == 'M' ? "Male.jpeg" : "Female.jpeg",
					Mark = x.Mark,
				})
                .First();
            return PartialView(FirstStudent);
        }
        public IActionResult getStudentWithMarkAtCourse(int CourseId)
        {
            var StudentMark = context.Students_Courses
                .Include(x => x.Student_ref).ThenInclude(x => x.Department_ref)
                .Where(x => x.Course_Id == CourseId)
                .Select(x => new StudentWithMarkInCourseVM
                {
					Name = x.Student_ref.StudentName,
                    gender = x.Student_ref.gender,
                    Age = x.Student_ref.Age,
                    Department = x.Student_ref.Department_ref.DepartmentName,
                    Mark = x.Mark,
				}).OrderByDescending(x => x.Mark).ToList();
            return PartialView(StudentMark);
        }



        public IActionResult AddNewCourse()
        {
            return View(new Course());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveNewCourse (Course course)
        {
            if (ModelState.IsValid)
            {
                context.Courses.Add(course);
                context.SaveChanges();
                return RedirectToAction("EnrollNewCourseToDepartment",new { CourseId = course.CourseId});
            }
            return View("AddNewCourse", course);
        }

        public IActionResult EnrollNewCourseToDepartment (int CourseId)
        {
            ViewBag.CourseId = CourseId;
            ViewBag.Departments = context.Departments.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult SaveEnrollNewCourseToDepartment (List<Department_Course> Department_Course)
        {
            // User Not Choose Any Department
            if (Department_Course.All(x => x.Department_Id == 0))
            {
                ViewBag.CourseId = Department_Course[0].Course_Id;
                ViewBag.Departments = context.Departments.ToList();
                ModelState.AddModelError(string.Empty,"Please Choose At Least One Department");
                return View("EnrollNewCourseToDepartment");             
            }
            // Remove Empty Check That DepartmentId = 0
            Department_Course.RemoveAll(x => x.Department_Id == 0);

            foreach (var item in Department_Course)
            {
                CourseLogic.EnrollStudentToNewCourse(item.Course_Id,item.Department_Id);
            }

            context.Departments_Courses.AddRange(Department_Course);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteCourse(int CourseId)
        {
            var course = context.Courses.FirstOrDefault(x => x.CourseId == CourseId);
            context.Courses.Remove(course);
            context.SaveChanges();
            return Content($"Course With Id {CourseId} Is Deleted");
        }

    }
}



