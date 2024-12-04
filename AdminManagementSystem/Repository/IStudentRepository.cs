using AdminManagementSystem.Models;

namespace AdminManagementSystem.Repository
{
    public interface IStudentRepository
    {
        List<Student> getStudentAtDepartment(int deptId);

        List<Student> getAllStudent();

        void SaveNewStudent(Student student);

        
        bool IsNameExistAtUpdateStudent(int id, string name);


        Student getStudentUsingName (string name);

        Student getStudentUsingId (int id);

        void DeleteStudent(int Id);

        void UpdateStudent (Student student);

        void UpdateStudentImage(int Id, string Image);


	}
}
