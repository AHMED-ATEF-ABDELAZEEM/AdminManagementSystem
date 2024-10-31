using AdminManagementSystem.Models;

namespace AdminManagementSystem.BussinessLogic
{
    public class StudentBussinessLogic
    {
        private AppDbContext context = new AppDbContext();

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

        public bool IsNameExistAtAddNewStudent (string name)
        {
            var student = context.Students.FirstOrDefault(x => x.StudentName == name);
            return student == null ? false : true;
        }

        public bool IsImageExistAtAddNewStudent (string Image)
        {
            var student = context.Students.FirstOrDefault(x => x.Image == Image);
            return student == null ? false : true;
        }

        public bool IsNameExistAtUpdateStudent (int id,string name)
        {
            // Check If Name Exist But Not In Current Object
            // return true If Name Exist In Two Element (Current Element,another)
            return context.Students.Any(x => x.StudentName == name && x.StudentId != id);
        }

        public bool IsImageExistAtUpdateStudent (int id, string image)
        {
            // Check If Image Exist But Not In Current Object
            // return true If Image Exist In Two Element (Current Element,another)
            return context.Students.Any(x => x.Image == image && x.StudentId != id);
        }
    }
}
