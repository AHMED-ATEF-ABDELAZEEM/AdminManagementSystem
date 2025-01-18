namespace AdminManagementSystem.ViewModel
{
    public class UpdateMarkAtCourseVM
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public List<StudentMarkVM> StudentMarks { get; set; }
    }
    public class StudentMarkVM
    {
        public string StudentId { get; set; }
        public string? StudentName { get; set; }
        public int OldMark {  get; set; }
        public int NewMark { get; set; }
    }
}
