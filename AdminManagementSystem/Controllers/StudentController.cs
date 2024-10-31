using AdminManagementSystem.BussinessLogic;
using AdminManagementSystem.Models;
using AdminManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AdminManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private AppDbContext context = new AppDbContext();
        private StudentBussinessLogic StudentLogic = new StudentBussinessLogic();
        private DepartmentBussinessLogic DepartmentLogic = new DepartmentBussinessLogic();

        public IActionResult getAllStudent()
        {
            return View(StudentLogic.getAllStudent());
        }

        //public IActionResult AddMarkForNewStudent(int StudentId, int DeptId)
        //{
        //    ViewBag.StudentId = StudentId;
        //    ViewBag.Courses = context.Departments_Courses
        //        .Where(x => x.Department_Id == DeptId)
        //        .Select(x => x.Course_ref).ToList();
        //    return View(new List<Student_Course>());
        //}

        //public IActionResult SaveStudentMark(List<Student_Course> Student_Course)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        context.Students_Courses.AddRange(Student_Course);
        //        context.SaveChanges();
        //        return RedirectToAction("getAllStudent");
        //    }
        //    return RedirectToAction("getAllStudent");
        //}

        public IActionResult AddNewStudent()
        {
            ViewBag.Departments = DepartmentLogic.getAllDepartment();

            return View(new Student());
        }

        public IActionResult SaveNewStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                StudentLogic.SaveNewStudent(student);
                MakeReferenceWithStudentAndCourse(student.StudentId, student.DeptId);
                return RedirectToAction("UpdateStudentMark", new { id = student.StudentId});
            }
            ViewBag.Departments = DepartmentLogic.getAllDepartment();
            return View("AddNewStudent", student);
        }

        private void MakeReferenceWithStudentAndCourse(int StudentId, int DeptId)
        {
            var Courses = context.Departments_Courses
                .Where(x => x.Department_Id == DeptId)
                .Select(x => x.Course_ref).ToList();
            List<Student_Course> student_Courses_List = new List<Student_Course>();
            foreach (var course in Courses)
            {
                var Student_Course = new Student_Course();

                Student_Course.Student_Id = StudentId;
                Student_Course.Course_Id = course.CourseId;
                Student_Course.Mark = 5;

                student_Courses_List.Add(Student_Course);

            }
            context.Students_Courses.AddRange(student_Courses_List);
            context.SaveChanges();
        }



        // Open Form With Current Student Mark
        public IActionResult UpdateStudentMark(int id)
        {
            var Marks = context.Students_Courses.Include(x => x.Course_ref)
                .Where(x => x.Student_Id == id).ToList();
            return View(Marks);
        }


        // Save Updated Mark That Come From Form
        public IActionResult SaveUpdatedMark(List<Student_Course> Student_Course)
        {
            if (ModelState.IsValid)
            {
                context.Students_Courses.UpdateRange(Student_Course);
                context.SaveChanges();
                return RedirectToAction("getAllStudent");
            }
            return View("UpdateStudentMark", Student_Course);
        }


       
        public IActionResult DeleteStudent(int id)
        {
            var DeleteStudent = context.Students
                .Select(x => new DeleteStudentVM
                {
                    Id = x.StudentId,
                    Name = x.StudentName,
                })
                .FirstOrDefault(x => x.Id == id);

            return View(DeleteStudent);
        }

        public IActionResult ConfirmDelete (int StudentId)
        {
            // Delete Student
            var student = context.Students.FirstOrDefault(x => x.StudentId == StudentId);
            context.Students.Remove(student);
            // Delete Related Courses
            var Courses = context.Students_Courses.Where(x => x.Student_Id == StudentId).ToList();
            context.Students_Courses.RemoveRange(Courses);
            // Save Change 
            context.SaveChanges();
            return RedirectToAction("getAllStudent");
        }


        public IActionResult getInformationAboutStudent (int id)
        {
            var student = context.Students.Include(x => x.Department_ref)
                .Select(x => new StudentInformationVM
                {
                    Id = x.StudentId,
                    Name = x.StudentName,
                    Age = x.Age,
                    gender = x.gender == 'M' ? "Male" : "Female",
                    Image = x.Image != null ?x.Image : x.gender == 'M' ? "Male.jpeg" : "Female.jpeg",
                    City = x.City,
                    Department = x.Department_ref.DepartmentName,

                })
                .FirstOrDefault(x => x.Id == id);
            return View(student);
        }

        public IActionResult UpdateStudentData (int id)
        {
            var student = context.Students.FirstOrDefault(x => x.StudentId == id);
            return View(student);
        }

        public IActionResult SaveUpdatedData(Student student)
        {
            if (ModelState.IsValid)
            {
                context.Students.Update(student);
                context.SaveChanges();
                return RedirectToAction("getInformationAboutStudent",new {id = student.StudentId});
            }
            return View("UpdateStudentData", student);
        }

    }
}
