
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
        private DepartmentService DepartmentService;

        public StudentController(StudentService StudentService, DepartmentService DepartmentService)
        {
            this.StudentService = StudentService;
            this.DepartmentService = DepartmentService;
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
                int NumberOfCourseAtDepartment = DepartmentService.getCoursesAtDepartment(Newstudent.Student.DeptId).Count();
                if (NumberOfCourseAtDepartment == 0)
                {
                    return RedirectToAction("getAllStudent");
                }
                else
                {
                    return RedirectToAction("UpdateStudentMark", new { StudentId = Newstudent.Student.StudentId });
                }
            }
            Newstudent.Departments = StudentService.getAllDepartment();
            return View("AddNewStudent", Newstudent);
        }


        public IActionResult ShowStudentMark (string StudentId)
        {

            var model = StudentService.getStudentMarkWithInformation(StudentId);

			return View(model);
		}

        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult UpdateStudentMark(string StudentId)
        {
            var Marks = StudentService.getAllStudentMark(StudentId);
            int TotalMark = Marks.Sum(x => x.Mark);

            string State = "";
            if (TotalMark == 0) State = "Add";
            else State = "Update";
            
            ViewBag.State = State;
            
            return View(Marks);
        }


        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult SaveUpdatedMark(List<Student_Course> Student_Course)
        {
            if (ModelState.IsValid)
            {
                StudentService.UpdateStudentMark(Student_Course);
				string StudentId = Student_Course[0].Student_Id;
                return RedirectToAction("ShowStudentMark",new {StudentId = StudentId });
            }
            return View("UpdateStudentMark", Student_Course);
        }


        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult DeleteStudent(string id)
        {
            var DeleteStudent = StudentService.getDeleteStudent(id);

            return View(DeleteStudent);
        }
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult ConfirmDelete (string StudentId)
        {
            // Delete Student And Delete Related Courses With Mark

            StudentService.DeleteStudent(StudentId);

            return RedirectToAction("getAllStudent");
        }


        public IActionResult getInformationAboutStudent (string id)
        {
            var model = StudentService.getAllStudentInformation(id);
            return View(model);
        }

        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult UpdateStudentData (string id)
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
        public  IActionResult UpdateStudentImage(string id, string imageName)
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
