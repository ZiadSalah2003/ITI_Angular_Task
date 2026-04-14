using Microsoft.EntityFrameworkCore;

namespace ITI_API.Repositories
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
		IQueryable<TEntity> GetQueryable();
		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<TEntity> GetByIdAsync(int id);
		Task AddAsync(TEntity entity);
		void Update(TEntity entity);
		Task DeleteAsync(int id);
	}
}
