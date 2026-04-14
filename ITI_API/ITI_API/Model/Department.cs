namespace ITI_API.Model
{
	public class Department
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsDeleted { get; set; } = false;
		public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
		public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
	}
}
