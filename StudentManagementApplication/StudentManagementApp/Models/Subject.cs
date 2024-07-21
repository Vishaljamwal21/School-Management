namespace StudentManagementApp.Models
{
	public class Subject
	{
		public int SubjectId { get; set; }

		public int? ClassId { get; set; }

		public string SubjectName { get; set; }

		public virtual Class Class { get; set; }
	}
}
