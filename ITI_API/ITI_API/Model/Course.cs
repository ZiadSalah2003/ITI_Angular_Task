namespace ITI_API.Model
{
	public class Course
	{
		public int Id { get; set; }
		public string? Crs_Name { get; set; }
		public string? Crs_Description { get; set; }
		public int Duration { get; set; }
		public int? DepartmentId { get; set; }
		public bool IsDeleted { get; set; } = false;
		public virtual Department? Department { get; set; }
		public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new HashSet<StudentCourse>();
	}
}
