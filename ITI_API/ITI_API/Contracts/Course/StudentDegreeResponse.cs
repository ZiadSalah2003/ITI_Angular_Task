namespace ITI_API.Contracts.Course
{
	public record StudentDegreeResponse
	{
		public string StudentId { get; set; } = string.Empty;
		public string? StudentName { get; set; }
		public decimal Degree { get; set; }
	}
}