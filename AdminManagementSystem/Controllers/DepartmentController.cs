using AdminManagementSystem.BussinessLogic;
using AdminManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {
        private AppDbContext context = new AppDbContext();
        private DepartmentBussinessLogic DepartmentLogic = new DepartmentBussinessLogic();
        private StudentBussinessLogic StudentLogic = new StudentBussinessLogic();
        private Department_Course_BussinessLogic Department_Course_Logic = new Department_Course_BussinessLogic();

        public IActionResult Index(int DeptId)
        {
            var Department = context.Departments.Include(x => x.Students_ref)
           .Include(x => x.Department_Course_ref)
           .FirstOrDefault(x => x.DepartmentId == DeptId);

            ViewBag.DepartmentInformation = Department;

            ViewBag.CountOfMaleStudent = Department.Students_ref.Where(x => x.gender == 'M').Count();
            ViewBag.CountOfFemaleStudent = Department.Students_ref.Where(x => x.gender == 'F').Count();

            return View(DepartmentLogic.getAllDepartment());
        }

        public IActionResult getStudentAtDepartment(int DeptId)
        {
            return PartialView(StudentLogic.getStudentAtDepartment(DeptId));
        }

        public IActionResult getCoursesAtDepartment(int DeptId)
        {
            return PartialView(Department_Course_Logic.getCoursesAtDepartment(DeptId));
        }

        public IActionResult InformationAboutDepartment(int DeptId)
        {
            var model = context.Departments.Include(x => x.Students_ref)
                .Include(x => x.Department_Course_ref)
                .FirstOrDefault(x => x.DepartmentId == DeptId);
            return PartialView(model);
        }

        public IActionResult getFirstStudentAtDepartment(int deptId)
        {
            var firstStudent = context.Students.Include(x => x.Department_ref)
                .Where(s => s.DeptId == deptId) // Filter students by department ID
                .Select(s => new
                {
                    Student = s,
                    TotalMarks = s.Student_Course_ref.Sum(sc => sc.Mark) // Sum marks for each student
                })
                .OrderByDescending(s => s.TotalMarks) // Order by total marks in descending order
                .Select(s => s.Student) // Select the student object
                .FirstOrDefault(); // Get the first student or null if none exist

            if (firstStudent.gender == 'M') firstStudent.Image = "Male.jpeg";
            else firstStudent.Image = "Female.jpeg";

            return PartialView(firstStudent);

        }

        public IActionResult getStudentMarkAtDepartment (int deptId)
        {
            var studentsWithTotalMarks = context.Students.Include(x => x.Department_ref)
            .Where(s => s.DeptId == deptId) // Filter by department
            .Select(s => new StudentWithTotalMark
            {
                StudentName = s.StudentName,
                gender = s.gender,
                Age = s.Age,
                TotalMark = s.Student_Course_ref.Sum(sc => sc.Mark) // Calculate total marks
            })
            .ToList();

            return View(studentsWithTotalMarks);
            // Now studentsWithTotalMarks contains the list of students with their total marks
        }
    }
}
