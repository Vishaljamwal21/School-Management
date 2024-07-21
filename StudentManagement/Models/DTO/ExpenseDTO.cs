namespace StudentManagement.Models.DTO
{
    public class ExpenseDTO
    {
        public int ExpenseId { get; set; }

        public int? ClassId { get; set; }

        public int? SubjectId { get; set; }

        public int? ChargeAmount { get; set; }
    }
}
