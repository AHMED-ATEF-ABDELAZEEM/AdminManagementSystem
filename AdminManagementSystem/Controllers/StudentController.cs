
using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using AdminManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AdminManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private AppDbContext context;
        private IStudentRepository StudentRepository;
        private IDepartmentRepository DepartmentRepository;

        public StudentController(AppDbContext context, IStudentRepository StudentRepository, IDepartmentRepository DepartmentRepository)
        {
            this.context = context;
            this.StudentRepository = StudentRepository;
            this.DepartmentRepository = DepartmentRepository;
        }

        public IActionResult getAllStudent()
        {
            return View(StudentRepository.getAllStudent());
        }

        public IActionResult AddNewStudent()
        {
            ViewBag.Departments = DepartmentRepository.getAllDepartment();

            return View(new Student());
        }

        public IActionResult SaveNewStudent(Student student)
        {
            if (StudentRepository.IsNameExistAtAddNewStudent(student.StudentName))
            {
                ModelState.AddModelError("StudentName", "This Name Is Already Exist Please Enter Different Name");
            }

            //if (student.Image != null && StudentLogic.IsImageExistAtAddNewStudent(student.Image))
            //{
            //    ModelState.AddModelError("Image", "This Image Is Already Use Please Choose Different Image");
            //}
            if (ModelState.IsValid)
            {
                StudentRepository.SaveNewStudent(student);
                MakeReferenceWithStudentAndCourse(student.StudentId, student.DeptId);
                return RedirectToAction("UpdateStudentMark", new { StudentId = student.StudentId});
            }
            ViewBag.Departments = DepartmentRepository.getAllDepartment();
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
                Student_Course.Mark = 0;

                student_Courses_List.Add(Student_Course);

            }
            context.Students_Courses.AddRange(student_Courses_List);
            context.SaveChanges();
        }



        // Open Form With Current Student Mark

        public IActionResult ShowStudentMark (int StudentId)
        {

            var Model = context.Students.Include(x => x.Department_ref)
                .Select(x => new ShowStudentMarkVM
                {
                    StudentId = x.StudentId,
                    StudentName = x.StudentName,
                    Department = x.Department_ref.DepartmentName,
                })
                .FirstOrDefault(x => x.StudentId ==  StudentId);

            Model.CourseWithMark = context.Students_Courses.Include(x => x.Course_ref)
	        .Where(x => x.Student_Id == StudentId)
            .Select(x => new CourseWithMarkVM
            {
                CourseName = x.Course_ref.CourseName,
                StudentMark = x.Mark,
            })
            .ToList();


			return View(Model);
		}

        public IActionResult UpdateStudentMark(int StudentId)
        {
            var Marks = context.Students_Courses.Include(x => x.Course_ref)
                .Where(x => x.Student_Id == StudentId).ToList();
            return View(Marks);
        }


        // Save Updated Mark That Come From Form
        public IActionResult SaveUpdatedMark(List<Student_Course> Student_Course)
        {
            if (ModelState.IsValid)
            {
                context.Students_Courses.UpdateRange(Student_Course);
                context.SaveChanges();
                int StudentId = Student_Course[0].Student_Id;
                return RedirectToAction("ShowStudentMark",new {StudentId = StudentId });
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
            if (StudentRepository.IsNameExistAtUpdateStudent(student.StudentId,student.StudentName))
            {
                ModelState.AddModelError("StudentName", "This Name Is Already Exist Please Enter Different Name");
            }

            //if (student.Image != null && StudentLogic.IsImageExistAtUpdateStudent(student.StudentId,student.Image))
            //{
            //    ModelState.AddModelError("Image", "This Image Is Already Use Please Choose Different Image");
            //}
        
            if (ModelState.IsValid)
            {
                context.Students.Update(student);
                context.SaveChanges();
                return RedirectToAction("getInformationAboutStudent",new {id = student.StudentId});
            }
            return View("UpdateStudentData", student);

        }

        [HttpPost]
        public  IActionResult UpdateStudentImage(int id, string imageName)
        {
            if (imageName != null)
            {
                var student =  context.Students.FirstOrDefault(x => x.StudentId == id);
                student.Image = imageName;
                context.SaveChanges();
                return RedirectToAction("getInformationAboutStudent", new { id = id });
            }
            return RedirectToAction("getInformationAboutStudent", new { id = id });
        }


    }
}
