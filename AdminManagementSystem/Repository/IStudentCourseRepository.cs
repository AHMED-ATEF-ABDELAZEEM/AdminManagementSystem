using AdminManagementSystem.Models;

namespace AdminManagementSystem.Repository
{
	public interface IStudentCourseRepository
	{
		void AddStudentMark(List<Student_Course> Enrollments);

		List<Student_Course> getAllMarkWithCourseForStudent(string StudentId);

		void UpdateStudentMark(List<Student_Course> Enrollments);

		void DeleteStudentMark(string StudentId);

		Student_Course getMaxMarkAtCourseWithStudentInformation(string CourseId);

		List<Student_Course> getAllMarkAtCourseWithStudentInformation (string CourseId);
	}
}
