using AdminManagementSystem.Models;
using AdminManagementSystem.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Repository
{
	public class StudentCourseRepository : IStudentCourseRepository
	{
		private AppDbContext context;
        public StudentCourseRepository(AppDbContext context)
        {
            this.context = context;
        }
        public void AddStudentMark(List<Student_Course> Enrollments)
		{
			context.AddRange(Enrollments);
			context.SaveChanges();
		}

		public void DeleteStudentMark(int StudentId)
		{
			var studentMark = getAllMarkWithCourseForStudent(StudentId);
			context.Students_Courses.RemoveRange(studentMark);
			context.SaveChanges();
		}

        public List<Student_Course> getAllMarkAtCourseWithStudentInformation(int CourseId)
        {
            return context.Students_Courses
                .Include(x => x.Student_ref).ThenInclude(x => x.Department_ref)
                .Where(x => x.Course_Id == CourseId)
				.ToList();
 
        }

        public List<Student_Course> getAllMarkWithCourseForStudent(int StudentId)
		{
			return context.Students_Courses.Include(x => x.Course_ref)
			.Where(x => x.Student_Id == StudentId)
			.ToList();
		}

        public Student_Course getMaxMarkAtCourseWithStudentInformation(int CourseId)
        {
            return context.Students_Courses
					.Include(x => x.Student_ref).ThenInclude(x => x.Department_ref)
					.Where(x => x.Course_Id == CourseId)
					.OrderByDescending(x => x.Mark)
					.First();
        }

        public void UpdateStudentMark(List<Student_Course> Enrollments)
		{
			context.Students_Courses.UpdateRange(Enrollments);
			context.SaveChanges();
		}
	}
}
