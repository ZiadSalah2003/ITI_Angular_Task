using ITI_API.Abstractions;
using ITI_API.Contracts.Common;
using ITI_API.Contracts.Department;

namespace ITI_API.Service
{
	public interface IDepartmentService
	{
		Task<Result<PaginatedList<DepartmentResponse>>> GetAllDepartmentsAsync(RequestFilters filters);
		Task<Result<DepartmentResponse>> GetDepartmentByIdAsync(int id);
		Task<Result<DepartmentResponse>> CreateDepartmentAsync(CreateDepartmentRequest request);
		Task<Result<DepartmentResponse>> UpdateDepartmentAsync(int id, UpdateDepartmentRequest request);
		Task<Result> DeleteDepartmentAsync(int id);
	}
}
