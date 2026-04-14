namespace ITI_API.Contracts.Course
{
	public record AssignCourseDegreesRequest
	{
		public int DepartmentId { get; set; }
		public int CourseId { get; set; }
		public List<StudentDegreeRequest> StudentDegrees { get; set; } = new();
	}
}
