using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using AdminManagementSystem.ViewModel;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace AdminManagementSystem.Services
{
    public class StudentService
    {
        private IStudentRepository StudentRepository;
        private IDepartmentRepository DepartmentRepository;
        private ICourseRepository CourseRepository;
        private IStudentCourseRepository StudentCourseRepository;
        public StudentService(IStudentRepository StudentRepository, IDepartmentRepository DepartmentRepository, ICourseRepository CourseRepository, IStudentCourseRepository StudentCourseRepository)
        {
            this.StudentRepository = StudentRepository;
            this.DepartmentRepository = DepartmentRepository;
            this.CourseRepository = CourseRepository;
            this.StudentCourseRepository = StudentCourseRepository;
        }

		private void AddDefualtMarkForNewStudent (int StudentId, int DeptId)
		{
			var Courses = CourseRepository.getAllCoursesAtDepartment(DeptId);
				
			List<Student_Course> student_Courses_List = new List<Student_Course>();
			foreach (var course in Courses)
			{
				var Student_Course = new Student_Course();

				Student_Course.Student_Id = StudentId;
				Student_Course.Course_Id = course.CourseId;
				Student_Course.Mark = 0;

				student_Courses_List.Add(Student_Course);

			}
            StudentCourseRepository.AddStudentMark(student_Courses_List);
		}

		public List<ShowStudentVM> getAllStudent ()
        {
            var Student = StudentRepository.getAllStudent();

            return Student.Select(x => new ShowStudentVM
            {
                Id = x.StudentId,
                Name = x.StudentName,
                Age = x.Age,
                gender = x.gender,
                City = x.City,
            }).ToList();

        }

       
        public List<Department> getAllDepartment ()
        {
              return DepartmentRepository.getAllDepartment();
        }

        public void SaveNewStudent(AddNewStudentVM NewStudent)
        {
			StudentRepository.SaveNewStudent(NewStudent.Student);
            // Add Zero As a Defualt Mark For New Student
			AddDefualtMarkForNewStudent(NewStudent.Student.StudentId, NewStudent.Student.DeptId);
		}

        private StudentInfoVM getStudentInformationToShowWithMark (int StudentId)
        {
            var student = StudentRepository.getStudentUsingId(StudentId);
            var model = new StudentInfoVM();
            model.StudentId = student.StudentId;
            model.StudentName = student.StudentName;
            model.Department = student.Department_ref.DepartmentName;

            return model;
        }

        private List<CourseWithMarkVM> getCourseWithMark (int StudentId)
        {
            var courseWithMark = StudentCourseRepository.getAllMarkWithCourseForStudent(StudentId);
            return courseWithMark.Select(x => new CourseWithMarkVM
            {
				CourseName = x.Course_ref.CourseName,
				StudentMark = x.Mark,
			}).ToList();

		}

        public ShowStudentMarkVM getStudentMarkWithInformation (int StudentId)
        {
            var model = new ShowStudentMarkVM();
            model.StudentInfo = getStudentInformationToShowWithMark (StudentId);
            model.CourseWithMark = getCourseWithMark (StudentId);
            return model;
        }

        public List <Student_Course> getAllStudentMark (int StudentId)
        {
            return StudentCourseRepository.getAllMarkWithCourseForStudent (StudentId);
        }

        public void UpdateStudentMark (List<Student_Course> student_course)
        {
			StudentCourseRepository.UpdateStudentMark (student_course);
        }

        public DeleteStudentVM getDeleteStudent (int Id)
        {
            var student = StudentRepository.getStudentUsingId (Id);
            var model = new DeleteStudentVM();
            model.Id = student.StudentId;
            model.Name = student.StudentName;
            return model;
        }

        public void DeleteStudent (int Id)
        {
            // Delete Student
            StudentRepository.DeleteStudent(Id);
            // Delete Mark For Student
            StudentCourseRepository.DeleteStudentMark (Id);
		}

        public StudentInformationVM getAllStudentInformation (int Id)
        {
            var student = StudentRepository.getStudentUsingId(Id);
            var model = new StudentInformationVM();

            model.Id = student.StudentId;
            model.Name = student.StudentName;
            model.Age = student.Age;
            model.gender = student.gender == 'M' ? "Male" : "Female";
            model.Image = student.Image != null ? student.Image : student.gender == 'M' ? "Male.jpeg" : "Female.jpeg";
            model.City = student.City;
            model.Department = student.Department_ref.DepartmentName;

            return model;
        }

        public Student getStudentUsingId (int Id)
        {
            return StudentRepository.getStudentUsingId (Id);
        }


		public bool IsNameExistAtAddNewStudent(string name)
		{
            Student student = StudentRepository.getStudentUsingName(name);
            return student != null ? true : false;
             
		}

		public bool IsNameExistAtUpdateStudent (int Id,string name)
        {
            bool exist = StudentRepository.IsNameExistAtUpdateStudent(Id,name);
            return exist;
        }

        public void UpdateStudent (Student student)
        {
            StudentRepository.UpdateStudent (student);
        }

        public void UpdateStudentImage (int Id,string Image)
        {
            StudentRepository.UpdateStudentImage (Id, Image);
        }
	}
}
