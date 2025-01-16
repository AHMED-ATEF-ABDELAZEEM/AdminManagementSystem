
using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using AdminManagementSystem.Services;
using AdminManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AdminManagementSystem.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {

        private StudentService StudentService;

        public StudentController(StudentService StudentService)
        {
            this.StudentService = StudentService;
        }

        public IActionResult getAllStudent()
        {
            var model = StudentService.getAllStudent();
            return View(model);
        }

        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult AddNewStudent()
        {

            var model = new AddNewStudentVM();
            model.Departments = StudentService.getAllDepartment();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult SaveNewStudent(AddNewStudentVM Newstudent)
        {
            if (StudentService.IsNameExistAtAddNewStudent(Newstudent.Student.StudentName))
            {
                ModelState.AddModelError("Student.StudentName", "This Name Is Already Exist Please Enter Different Name");
            }

            if (ModelState.IsValid)
            {
                StudentService.SaveNewStudent(Newstudent);
                return RedirectToAction("UpdateStudentMark", new { StudentId = Newstudent.Student.StudentId});
            }
            Newstudent.Departments = StudentService.getAllDepartment();
            return View("AddNewStudent", Newstudent);
        }


        public IActionResult ShowStudentMark (int StudentId)
        {

            var model = StudentService.getStudentMarkWithInformation(StudentId);

			return View(model);
		}

        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult UpdateStudentMark(int StudentId)
        {
            var Marks = StudentService.getAllStudentMark(StudentId);
            
            return View(Marks);
        }


        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult SaveUpdatedMark(List<Student_Course> Student_Course)
        {
            if (ModelState.IsValid)
            {
                StudentService.UpdateStudentMark(Student_Course);
                int StudentId = Student_Course[0].Student_Id;
                return RedirectToAction("ShowStudentMark",new {StudentId = StudentId });
            }
            return View("UpdateStudentMark", Student_Course);
        }


        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult DeleteStudent(int id)
        {
            var DeleteStudent = StudentService.getDeleteStudent(id);

            return View(DeleteStudent);
        }
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult ConfirmDelete (int StudentId)
        {
            // Delete Student And Delete Related Courses With Mark

            StudentService.DeleteStudent(StudentId);

            return RedirectToAction("getAllStudent");
        }


        public IActionResult getInformationAboutStudent (int id)
        {
            var model = StudentService.getAllStudentInformation(id);
            return View(model);
        }

        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult UpdateStudentData (int id)
        {
            var model = StudentService.getStudentUsingId(id);
            return View(model);
        }



		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult SaveUpdatedData(Student student)
        {
            if (StudentService.IsNameExistAtUpdateStudent(student.StudentId,student.StudentName))
            {
                ModelState.AddModelError("StudentName", "This Name Is Already Exist Please Enter Different Name");
            }

        
            if (ModelState.IsValid)
            {
                StudentService.UpdateStudent(student);
                return RedirectToAction("getInformationAboutStudent",new {id = student.StudentId});
            }
            return View("UpdateStudentData", student);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin,Admin")]
        public  IActionResult UpdateStudentImage(int id, string imageName)
        {
            if (imageName != null)
            {
                StudentService.UpdateStudentImage(id, imageName);
                return RedirectToAction("getInformationAboutStudent", new { id = id });
            }
            return RedirectToAction("getInformationAboutStudent", new { id = id });
        }


    }
}
