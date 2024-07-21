namespace StudentManagement.Models.DTO
{
    public class TeacherAttendanceDTO
    {
        public int Id { get; set; }

        public int? TeacherId { get; set; }

        public bool? Status { get; set; }

        public DateOnly? Date { get; set; }
    }
}
