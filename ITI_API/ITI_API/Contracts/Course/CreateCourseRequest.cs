namespace ITI_API.Contracts.Course
{
	public record CreateCourseRequest
	{
		public string? Crs_Name { get; set; }
		public string? Crs_Description { get; set; }
		public int Duration { get; set; }
		public int? DepartmentId { get; set; }
		public List<StudentDegreeRequest>? StudentDegrees { get; set; }
	}
}