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

        public List<Student> getStudentAtDepartment (int deptId)
        {
            return context.Students.Where(x => x.DeptId == deptId).ToList();
        }

        public List<Student> getAllStudent ()
        {
            return context.Students.ToList();
        }

        public void SaveNewStudent (Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
        }

        public Student getStudentUsingName (string name)
        {
			return context.Students.FirstOrDefault(x => x.StudentName.ToLower() == name.ToLower());
		}

        public bool IsNameExistAtUpdateStudent(int id, string name)
        {
            // Check If Name Exist But Not In Current Object
            // return true If Name Exist In Two Element (Current Element,another)
            return context.Students.Any(x => x.StudentName == name && x.StudentId != id);
        }

		public Student getStudentUsingId(int id)
        {
			return context.Students.Include(x => x.Department_ref)
	         .FirstOrDefault(x => x.StudentId == id);
		}

        public void DeleteStudent (int Id)
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

		public void UpdateStudentImage(int Id, string Image)
		{
			var student = getStudentUsingId (Id);
            student.Image = Image;
            context.SaveChanges();
		}

        public List<Student> getStudentAtCourse(int CourseId)
        {
            return context.Students_Courses.Include(x => x.Student_ref)
             .Where(x => x.Course_Id == CourseId).Select(x => x.Student_ref).ToList();
        }

        //public Student getFirstStudentAtCourse(int CourseId)
        //{
        //    return context.Students_Courses
        //        .Include(x => x.Student_ref)
        //        .Where(x => x.Course_Id == CourseId)
        //        .OrderByDescending(x => x.Mark)
        //        .Select(x => x.Student_ref)
        //        .First();
        //}
    }
}
