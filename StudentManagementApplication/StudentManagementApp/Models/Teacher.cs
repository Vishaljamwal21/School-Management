namespace StudentManagementApp.Models
{
	public class Teacher
	{
		public int TeacherId { get; set; }

		public string Name { get; set; }

		public DateOnly? Dob { get; set; }

		public string Gender { get; set; }

		public long? Mobile { get; set; }

		public string Email { get; set; }

		public string Address { get; set; }

		public string Password { get; set; }

		public string Role { get; set; }

		public string Token { get; set; }
	}
}
