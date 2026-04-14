namespace ITI_API.Contracts.Course
{
	public class StudentDegreeRequest
	{
		public string StudentId { get; set; } = string.Empty;
		public decimal Degree { get; set; }
	}
}
