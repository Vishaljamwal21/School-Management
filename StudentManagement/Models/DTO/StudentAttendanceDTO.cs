namespace StudentManagement.Models.DTO
{
    public class StudentAttendanceDTO
    {
        public int Id { get; set; }

        public int? ClassId { get; set; }

        public int? SubjectId { get; set; }

        public string RollNo { get; set; }

        public bool? Status { get; set; }

        public DateOnly? Date { get; set; }
    }
}
