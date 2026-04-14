using ITI_API.Model;

namespace ITI_API.Repositories
{
	public interface IUnitOfWork
	{
		IGenericRepository<Course> Courses { get; }
		IGenericRepository<Student> Students { get; }
		IGenericRepository<Department> Departments { get; }
		Task SaveAsync();
	}
}
