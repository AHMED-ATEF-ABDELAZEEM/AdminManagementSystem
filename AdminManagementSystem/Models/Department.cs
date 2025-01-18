using System.ComponentModel.DataAnnotations;

namespace AdminManagementSystem.Models
{
    public class Department
    {
        public string? DepartmentId { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string DepartmentName { get; set; }
		[Required]
		[MinLength(7)]
		[MaxLength(25)]
		public string DepartmentManager {  get; set; }
        public virtual List<Department_Course>? Department_Course_ref {  get; set; }
        public virtual List<Student>? Students_ref { get; set; }

    }
}
