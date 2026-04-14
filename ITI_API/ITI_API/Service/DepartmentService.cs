using ITI_API.Abstractions;
using ITI_API.Contracts.Common;
using ITI_API.Contracts.Department;
using ITI_API.Errors;
using ITI_API.Model;
using ITI_API.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ITI_API.Service
{
	public class DepartmentService : IDepartmentService
	{
		private readonly IUnitOfWork _unitOfWork;
		
		public DepartmentService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PaginatedList<DepartmentResponse>>> GetAllDepartmentsAsync(RequestFilters filters)
		{
			var departments = _unitOfWork.Departments.GetQueryable()
				.Where(d => !d.IsDeleted)
				.Select(d => new DepartmentResponse(
					d.Id,
					d.Name,
					d.Students.Count,
					d.Students.Select(s => s.Name).ToList()));

			var response = await PaginatedList<DepartmentResponse>.CreateAsync(departments, filters.PageNumber, filters.PageSize);
			return Result.Success(response);
		}

		public async Task<Result<DepartmentResponse>> GetDepartmentByIdAsync(int id)
		{
			var department = await _unitOfWork.Departments.GetByIdAsync(id);
			if (department == null || department.IsDeleted)
				return Result.Failure<DepartmentResponse>(DepartmentErrors.DepartmentNotFound);

			var response = new DepartmentResponse(
				department.Id,
				department.Name,
				department.Students.Count,
				department.Students.Select(s => s.Name).ToList());

			return Result.Success(response);
		}

		public async Task<Result<DepartmentResponse>> CreateDepartmentAsync(CreateDepartmentRequest request)
		{
			var departmentNameExists = await _unitOfWork.Departments.GetQueryable()
				.AnyAsync(d => !d.IsDeleted && d.Name == request.Name);

			if (departmentNameExists)
				return Result.Failure<DepartmentResponse>(DepartmentErrors.DepartmentNameAlreadyExists);

			var department = request.Adapt<Department>();
			await _unitOfWork.Departments.AddAsync(department);
			await _unitOfWork.SaveAsync();

			var response = new DepartmentResponse(department.Id, department.Name, 0, new List<string>());
			return Result.Success(response);
		}

		public async Task<Result<DepartmentResponse>> UpdateDepartmentAsync(int id, UpdateDepartmentRequest request)
		{
			var department = await _unitOfWork.Departments.GetByIdAsync(id);
			if (department == null || department.IsDeleted)
				return Result.Failure<DepartmentResponse>(DepartmentErrors.DepartmentNotFound);

			var departmentNameExists = await _unitOfWork.Departments.GetQueryable()
				.AnyAsync(d => d.Id != id && !d.IsDeleted && d.Name == request.Name);

			if (departmentNameExists)
				return Result.Failure<DepartmentResponse>(DepartmentErrors.DepartmentNameAlreadyExists);

			request.Adapt(department);
			_unitOfWork.Departments.Update(department);
			await _unitOfWork.SaveAsync();

			var response = new DepartmentResponse(
				department.Id,
				department.Name,
				department.Students.Count,
				department.Students.Select(s => s.Name).ToList());
			return Result.Success(response);
		}

		public async Task<Result> DeleteDepartmentAsync(int id)
		{
			var department = await _unitOfWork.Departments.GetByIdAsync(id);
			if (department == null || department.IsDeleted)
				return Result.Failure(DepartmentErrors.DepartmentNotFound);

			department.IsDeleted = true;
			_unitOfWork.Departments.Update(department);
			await _unitOfWork.SaveAsync();
			return Result.Success();
		}
	}
}
