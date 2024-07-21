namespace StudentManagementApp.Models
{
	public class TeacherSubject
	{
		public int Id { get; set; }

		public int? ClassId { get; set; }

		public int? SubjectId { get; set; }

		public int? TeacherId { get; set; }

		public virtual Class Class { get; set; }

		public virtual Subject Subject { get; set; }

		public virtual Teacher Teacher { get; set; }
	}
}
