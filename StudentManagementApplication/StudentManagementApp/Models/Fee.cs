namespace StudentManagementApp.Models
{
	public class Fee
	{
		public int FeesId { get; set; }

		public int? ClassId { get; set; }

		public int? FeesAmount { get; set; }

		public virtual Class Class { get; set; }
	}
}
