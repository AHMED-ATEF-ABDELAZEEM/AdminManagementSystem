namespace AdminManagementSystem.ViewModel
{
    public class ShowStudentMarkVM
    {

        public StudentInfoVM StudentInfo { get; set; }
		public List<CourseWithMarkVM> CourseWithMark { get; set; }
    }
    public class CourseWithMarkVM
    {
        public string CourseName { get; set; }
        public int StudentMark { get; set; }
    }

    public class StudentInfoVM
    {
		public int StudentId { get; set; }
		public string StudentName { get; set; }
		public string Department { get; set; }
	}
}
