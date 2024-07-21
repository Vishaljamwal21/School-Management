using System.ComponentModel.DataAnnotations;

namespace StudentManagementApp.Models
{
	public class Class
	{
		public int ClassId { get; set; }
		[Required]
		public string ClassName { get; set; }
	}
}
