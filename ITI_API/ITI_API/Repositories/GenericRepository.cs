using ITI_API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITI_API.Repositories
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		private readonly ApplicationDbContext _context;

		public GenericRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public IQueryable<TEntity> GetQueryable()
		{
			return _context.Set<TEntity>().AsQueryable();
		}
		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await _context.Set<TEntity>().ToListAsync();
		}
		public async Task<TEntity> GetByIdAsync(int id)
		{
			return await _context.Set<TEntity>().FindAsync(id);
		}
		public async Task AddAsync(TEntity entity)
		{
			await _context.Set<TEntity>().AddAsync(entity);
		}
		public void Update(TEntity entity)
		{
			_context.Set<TEntity>().Update(entity);
		}
		public async Task DeleteAsync(int id)
		{
			var entity = await GetByIdAsync(id);
			if (entity != null)
			{
				_context.Set<TEntity>().Remove(entity);
			}
		}
	}
}
