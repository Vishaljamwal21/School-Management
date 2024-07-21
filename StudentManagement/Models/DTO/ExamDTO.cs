namespace StudentManagement.Models.DTO
{
    public class ExamDTO
    {
        public int ExamId { get; set; }

        public int? ClassId { get; set; }

        public int? SubjectId { get; set; }

        public string RollNo { get; set; }

        public int? TotalMarks { get; set; }

        public int? OutOfMarks { get; set; }
    }
}
