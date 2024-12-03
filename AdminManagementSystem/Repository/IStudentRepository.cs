using AdminManagementSystem.Models;

namespace AdminManagementSystem.Repository
{
    public interface IStudentRepository
    {
        List<Student> getStudentAtDepartment(int deptId);

        List<Student> getAllStudent();

        void SaveNewStudent(Student student);

        bool IsNameExistAtAddNewStudent(string name);

        bool IsImageExistAtAddNewStudent(string Image);


        bool IsNameExistAtUpdateStudent(int id, string name);

        bool IsImageExistAtUpdateStudent(int id, string image);

    }
}
