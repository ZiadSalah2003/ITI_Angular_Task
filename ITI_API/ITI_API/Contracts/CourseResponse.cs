using ITI_API.Contracts.Course;

namespace ITI_API.Contracts.Course
{
	public record CourseResponse
	{
		public int Id { get; set; }
		public string? Crs_Name { get; set; }
		public string? Crs_Description { get; set; }
		public int Duration { get; set; }
		public int? DepartmentId { get; set; }
		public string? DepartmentName { get; set; }
		public List<StudentDegreeResponse> StudentDegrees { get; set; } = new();
	}
}
