using Microsoft.AspNetCore.Identity;

namespace ITI_API.Model
{
	public class Student : IdentityUser
	{
		public string Name { get; set; }
		public int Age { get; set; }
		public int? DepartmentId { get; set; }
		public bool IsDeleted { get; set; } = false;
		public virtual Department Department { get; set; }
		public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new HashSet<StudentCourse>();
	}
}
