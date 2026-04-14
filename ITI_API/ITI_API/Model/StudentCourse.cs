namespace ITI_API.Model
{
	public class StudentCourse
	{
		public string StudentId { get; set; } = string.Empty;
		public int CourseId { get; set; }
		public decimal Degree { get; set; }
		public virtual Student Student { get; set; } = default!;
		public virtual Course Course { get; set; } = default!;
	}
}