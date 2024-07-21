namespace StudentManagement.Models.DTO
{
    public class TeacherSubjectDTO
    {
        public int Id { get; set; }

        public int? ClassId { get; set; }

        public int? SubjectId { get; set; }

        public int? TeacherId { get; set; }
    }
}
