namespace AdminManagementSystem.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentManager {  get; set; }
        public virtual List<Department_Course>? Department_Course_ref {  get; set; }
        public virtual List<Student>? Students_ref { get; set; }

    }
}
