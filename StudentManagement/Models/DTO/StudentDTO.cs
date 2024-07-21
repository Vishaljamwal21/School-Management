namespace StudentManagement.Models.DTO
{
    public class StudentDTO
    {
        public int StudentId { get; set; }

        public string Name { get; set; }

        public DateOnly? Dob { get; set; }

        public string Gender { get; set; }

        public long? Mobile { get; set; }

        public string RollNo { get; set; }

        public string Address { get; set; }

        public int? ClassId { get; set; }
    }
}
