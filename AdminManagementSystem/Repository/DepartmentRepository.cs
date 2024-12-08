using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using AdminManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
		private AppDbContext context;

        public DepartmentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Department getDepartment (int deptId)
		{
			var Department = context.Departments.Include(x => x.Students_ref)
			.Include(x => x.Department_Course_ref)
			.FirstOrDefault(x => x.DepartmentId == deptId);

			return Department;
		}

		public Department getFirstDepartment()
		{
			var FirstDepartment = context.Departments.Include(x => x.Students_ref)
			.Include(x => x.Department_Course_ref)
			.First();

			return FirstDepartment;
		}

        public List<Department> getAllDepartment ()
        {
            return context.Departments.ToList();
        }

        public List<Student> getStudentsAtDepartment (int deptId)
        {
			return context.Students.Where(x => x.DeptId == deptId).ToList();
		}

        public List<Course> getCoursesAtDepartment (int deptId)
        {
			var Courses =  context.Departments_Courses.Include(x => x.Course_ref)
             .Where(x => x.Department_Id == deptId).Select(x => x.Course_ref).ToList();

			return Courses;
        }

        public Department getInformationAboutDepartment (int deptId) 
        {
			var Department = context.Departments.Include(x => x.Students_ref)
            .Include(x => x.Department_Course_ref)
            .FirstOrDefault(x => x.DepartmentId == deptId);

			return Department;
		}

        public Student getFirstStudentAtDepartment(int deptId)
        {
			var FirstStudent = context.Students.Include(x => x.Department_ref)
			.Where(s => s.DeptId == deptId) // Filter students by department ID
			.Select(s => new
			{
				Student = s,
				TotalMarks = s.Student_Course_ref.Sum(sc => sc.Mark) // Sum marks for each student
			})
			.OrderByDescending(s => s.TotalMarks) // Order by total marks in descending order
			.Select(s => s.Student) // Select the student object
			.FirstOrDefault(); // Get the first student or null if none exist

			// Add Defualt Image If It Null
			if (FirstStudent.Image == null)
			{
				if (FirstStudent.gender == 'M') FirstStudent.Image = "Male.jpeg";
				else FirstStudent.Image = "Female.jpeg";
			}


			return FirstStudent;
		}

		public List<StudentWithTotalMark>   getStudentMarkAtDepartment(int deptId)
		{
			var StudentsWithTotalMarks = context.Students.Include(x => x.Department_ref)
			.Where(s => s.DeptId == deptId) // Filter by department
			.Select(s => new StudentWithTotalMark
			{
				StudentName = s.StudentName,
				gender = s.gender,
				Age = s.Age,
				TotalMark = s.Student_Course_ref.Sum(sc => sc.Mark) // Calculate total marks
			}).OrderByDescending(x => x.TotalMark)
			.ToList();

			return StudentsWithTotalMarks;

		}

        public List<Department> getDepartmentThatCourseExistInIt(int CourseId)
        {
            return context.Departments_Courses.Include(x => x.Department_ref)
            .Where(x => x.Course_Id == CourseId).Select(x => x.Department_ref).ToList();
        }
    }
}
