﻿using AdminManagementSystem.Models;

namespace AdminManagementSystem.Repository
{
	public interface IStudentCourseRepository
	{
		void AddStudentMark(List<Student_Course> Enrollments);

		List<Student_Course> getAllMarkWithCourseForStudent(int StudentId);

		void UpdateStudentMark(List<Student_Course> Enrollments);

		void DeleteStudentMark(int StudentId);
	}
}
