using ITI_API.Model;
using ITI_API.Persistence;

namespace ITI_API.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private IGenericRepository<Course>? _courses;
		private IGenericRepository<Student>? _students;
		private IGenericRepository<Department>? _departments;
		private readonly ApplicationDbContext _context;
		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
		}
		public IGenericRepository<Course> Courses
		{
			get
			{
				return _courses ??= new GenericRepository<Course>(_context);
			}
		}
		public IGenericRepository<Student> Students { 
			get 
			{
				return _students ??= new GenericRepository<Student>(_context);
			} 
		}
		public IGenericRepository<Department> Departments
		{
			get
			{
				return _departments ??= new GenericRepository<Department>(_context);
			}
		}
		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
