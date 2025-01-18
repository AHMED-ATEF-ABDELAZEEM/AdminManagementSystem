using AdminManagementSystem.Models;

namespace AdminManagementSystem.Repository
{
    public interface IStudentRepository
    {
        List<Student> getStudentAtDepartment(string deptId);

        List<Student> getAllStudent();

        void SaveNewStudent(Student student);

        
        bool IsNameExistAtUpdateStudent(string id, string name);


        Student getStudentUsingName (string name);

        Student getStudentUsingId (string id);

        void DeleteStudent(string Id);

        void UpdateStudent (Student student);

        void UpdateStudentImage(string Id, string Image);

        List <Student> getStudentAtCourse (string courseId);


	}
}
