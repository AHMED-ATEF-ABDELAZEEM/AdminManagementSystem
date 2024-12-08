using AdminManagementSystem.Models;
using AdminManagementSystem.ViewModel;

namespace AdminManagementSystem.Extension
{
    public static class CourseServiceExtension
    {
        public static List<StudentWithMarkInCourseVM> MapToStudentWithMarkInCourseVM(this List<Student_Course> MarkWithStudent)
        {
            return MarkWithStudent.Select(x => new StudentWithMarkInCourseVM
            {
                Name = x.Student_ref.StudentName,
                Age = x.Student_ref.Age,
                gender = x.Student_ref.gender,
                Department = x.Student_ref.Department_ref.DepartmentName,
                Mark = x.Mark,
            }).ToList();
        }

        public static List<Student_Course> MapToStudent_Course(this List<StudentMarkVM> StudentMark,int CourseId)
        {
            return StudentMark.Select(x => new Student_Course
            {
                Course_Id = CourseId,
                Student_Id = x.StudentId,
                Mark = x.NewMark,
            }).ToList();
        }

        public static List<StudentMarkVM> MapToStudentMarkVM(this List<Student_Course> Student_Course)
        {

            return Student_Course.Select(x => new StudentMarkVM
            {
                StudentId = x.Student_Id,
                StudentName = x.Student_ref.StudentName,
                OldMark = x.Mark,
                NewMark = x.Mark,
            }).ToList();
        }

        public static List<Student_Course> MapToStudent_Course(this List<Student> Students,int CourseId)
        {
            return Students.Select(x => new Student_Course
            {
                Student_Id = x.StudentId,
                Course_Id = CourseId,
                Mark = 0,
            }).ToList();
        }

        public static FirstStudentWithMarkVM MapToFirstStudentWithMarkVM (this Student_Course MaxMarkWithStudent)
        {
            var Model = new FirstStudentWithMarkVM();

            Model.Name = MaxMarkWithStudent.Student_ref.StudentName;
            Model.gender = MaxMarkWithStudent.Student_ref.gender == 'M' ? "Male" : "Female";
            Model.Age = MaxMarkWithStudent.Student_ref.Age;
            Model.Department = MaxMarkWithStudent.Student_ref.Department_ref.DepartmentName;
            Model.Image = MaxMarkWithStudent.Student_ref.Image != null ? MaxMarkWithStudent.Student_ref.Image : MaxMarkWithStudent.Student_ref.gender == 'M' ? "Male.jpeg" : "Female.jpeg";
            Model.Mark = MaxMarkWithStudent.Mark;

            return Model;
        }
    }
}
