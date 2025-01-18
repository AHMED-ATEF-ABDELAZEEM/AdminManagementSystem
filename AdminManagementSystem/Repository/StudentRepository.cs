using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using AdminManagementSystem.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private AppDbContext context;

        public StudentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Student> getStudentAtDepartment (string deptId)
        {
            return context.Students.Where(x => x.DeptId == deptId).ToList();
        }

        public List<Student> getAllStudent ()
        {
            return context.Students.ToList();
        }

        public void SaveNewStudent (Student student)
        {
            student.StudentId = Guid.NewGuid().ToString();
            context.Students.Add(student);
            context.SaveChanges();
        }

        public Student getStudentUsingName (string name)
        {
			return context.Students.FirstOrDefault(x => x.StudentName.ToLower() == name.ToLower());
		}

        public bool IsNameExistAtUpdateStudent(string id, string name)
        {
            // Check If Name Exist But Not In Current Object
            // return true If Name Exist In Two Element (Current Element,another)
            return context.Students.Any(x => x.StudentName == name && x.StudentId != id);
        }

		public Student getStudentUsingId(string id)
        {
			return context.Students.Include(x => x.Department_ref)
	         .FirstOrDefault(x => x.StudentId == id);
		}

        public void DeleteStudent (string Id)
        {
            var Student = getStudentUsingId (Id);
            context.Students.Remove(Student);
            context.SaveChanges();
        }

		public void UpdateStudent(Student student)
		{
			context.Students.Update(student);
            context.SaveChanges ();
		}

		public void UpdateStudentImage(string Id, string Image)
		{
			var student = getStudentUsingId (Id);
            student.Image = Image;
            context.SaveChanges();
		}

        public List<Student> getStudentAtCourse(string CourseId)
        {
            return context.Students_Courses.Include(x => x.Student_ref)
             .Where(x => x.Course_Id == CourseId).Select(x => x.Student_ref).ToList();
        }
    }
}
