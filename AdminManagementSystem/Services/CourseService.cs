﻿using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using AdminManagementSystem.ViewModel;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using AdminManagementSystem.Extension;

namespace AdminManagementSystem.Services
{
    public class CourseService
    {
        private ICourseRepository CourseRepository;
        private IStudentRepository StudentRepository;
        private IDepartmentRepository DepartmentRepository;
        private IStudentCourseRepository StudentCourseRepository;
        public CourseService(ICourseRepository CourseRepository, IStudentRepository StudentRepository, IDepartmentRepository DepartmentRepository, IStudentCourseRepository StudentCourseRepository)
        {
            this.CourseRepository = CourseRepository;
            this.StudentRepository = StudentRepository;
            this.DepartmentRepository = DepartmentRepository;
            this.StudentCourseRepository = StudentCourseRepository;
        }

        public string getIdForFirstCourse ()
        {
            var course = CourseRepository.getFirstCourse();
            return course.CourseId;
        }

        public CourseInformationVM getCourseInformation(string CourseId)
        {
            var Model = new CourseInformationVM();
            var Course = CourseRepository.getCourseUsingId(CourseId);

            Model.Course = Course;

            Model.NumberOfDepartment = Course.Department_Course_ref.Count();

            Model.NumberOfStudent = Course.Student_Course_ref.Count();
            Model.NumberOfMaleStudent = Course.Student_Course_ref.Where(x => x.Student_ref.gender == 'M').Count();
            Model.NumberOfFemaleStudent= Model.NumberOfStudent - Model.NumberOfMaleStudent;


            return Model;
		}

        public List<Course> getAllCourses ()
        {
            return CourseRepository.getAllCourses();
        }

        public List<Student> getStudentAtCourse (string CourseId)
        {
            return StudentRepository.getStudentAtCourse(CourseId);
        }

        public List<Department> getDepartmentThatCourseExistInIt (string CourseId)
        {
            return DepartmentRepository.getDepartmentThatCourseExistInIt(CourseId);
        }

        public FirstStudentWithMarkVM getFirstStudentAtCourse(string CourseId)
        {
            var MaxMarkWithStudent = StudentCourseRepository.getMaxMarkAtCourseWithStudentInformation(CourseId);

            return MaxMarkWithStudent.MapToFirstStudentWithMarkVM();
        }



        public List<StudentWithMarkInCourseVM> getAllStudentWithMarkAtCourse (string CourseId)
        {
            var MarkWithStudent = StudentCourseRepository.getAllMarkAtCourseWithStudentInformation(CourseId);

            var StudentWithMarkList = MarkWithStudent.MapToStudentWithMarkInCourseVM();

            return StudentWithMarkList;

        }

        public List<Department> GetAllDepartment ()
        {
            return DepartmentRepository.getAllDepartment();
        }

        private void EnrollStudentToNewCourse(string CourseId, string DeptId)
        {
            
            var Students = StudentRepository.getStudentAtDepartment(DeptId);

            var Student_Course_List = Students.MapToStudent_Course(CourseId);

            CourseRepository.SaveEnrollmentStudentToCourse(Student_Course_List);
        }

        private void RegisterCourseToDepartment(string CourseId, string DeptId)
        {
            var DepartmentCourse = new Department_Course();
            DepartmentCourse.Course_Id = CourseId;
            DepartmentCourse.Department_Id = DeptId;
            CourseRepository.RegisterCourseToDepartment(DepartmentCourse);
        }

        public void SaveCourseAndRegisterToDepartmentAndEnrollmentStudent(AddCourseAndAssignToDepartmentVM Model)
        {  
            CourseRepository.SaveNewCourse(Model.NewCourse);
			string CourseId = Model.NewCourse.CourseId;
            foreach (var deptId in Model.AssignToDepartment)
            {
                RegisterCourseToDepartment(CourseId, deptId);
                EnrollStudentToNewCourse(CourseId, deptId);
            }
        }

        public bool IsCourseExist (string CourseName)
        {
           return CourseRepository.IsCourseExist(CourseName);
        }



        public string GetCourseName(string CourseId)
        {
            return CourseRepository.getCourseUsingId(CourseId).CourseName;
        }

        public List<StudentMarkVM> getStudentMarkAtCourse (string CourseId)
        {
            var Student_Course = StudentCourseRepository.getAllMarkAtCourseWithStudentInformation(CourseId);

            return Student_Course.MapToStudentMarkVM();
        }



        public int UpdateStudentMark (UpdateMarkAtCourseVM Model)
        {
            var ChangedMark = Model.StudentMarks.Where(x => x.NewMark != x.OldMark).ToList();
            if (ChangedMark.Count == 0) return 0; // No Mark Is Updated


            var StudentCourseList = ChangedMark.MapToStudent_Course(Model.CourseId);

            StudentCourseRepository.UpdateStudentMark(StudentCourseList);

            // return Number Of Updated Mark
            return ChangedMark.Count;
		}

    }


}
