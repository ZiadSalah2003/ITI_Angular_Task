using ITI_API.Abstractions;
using ITI_API.Contracts.Common;
using ITI_API.Contracts.Student;
using ITI_API.Errors;
using ITI_API.Model;
using ITI_API.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ITI_API.Service
{
	public class StudentService : IStudentService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<Student> _userManger;

		public StudentService(IUnitOfWork unitOfWork, UserManager<Student> userManger)
		{
			_unitOfWork = unitOfWork;
			_userManger = userManger;
		}

		public async Task<Result<PaginatedList<StudentResponse>>> GetAllStudentsAsync(RequestFilters filters)
		{
			var students = _unitOfWork.Students.GetQueryable().ProjectToType<StudentResponse>();
			var response = await PaginatedList<StudentResponse>.CreateAsync(students, filters.PageNumber, filters.PageSize);
			return Result.Success(response);
		}

		public async Task<Result<StudentResponse>> GetStudentByIdAsync(string? studentId, string id)
		{
			var user = _userManger.Users.FirstOrDefault(u => u.Id == studentId);
			if(user == null)
				return Result.Failure<StudentResponse>(StudentErros.StudentNotFound);
			var Roles = await _userManger.GetRolesAsync(user);
			if (user.Id != id)
				return Result.Failure<StudentResponse>(UserErrors.InvalidCredentails);

			var student = _userManger.Users.FirstOrDefault(u => u.Id == id);
			if (student == null)
				return Result.Failure<StudentResponse>(StudentErros.StudentNotFound);

			return Result.Success(student.Adapt<StudentResponse>());
		}

		public async Task<Result<StudentResponse>> CreateStudentAsync(CreateStudentRequest request)
		{
			var student = request.Adapt<Student>();
			await _unitOfWork.Students.AddAsync(student);
			await _unitOfWork.SaveAsync();

			return Result.Success(student.Adapt<StudentResponse>());
		}

		public async Task<Result<StudentResponse>> UpdateStudentAsync(string id, UpdateStudentRequest request)
		{
			var student = await _userManger.Users.FirstOrDefaultAsync(s => s.Id == id);
			if (student == null)
				return Result.Failure<StudentResponse>(StudentErros.StudentNotFound);

			request.Adapt(student);
			var updateResult = await _userManger.UpdateAsync(student);
			if (!updateResult.Succeeded)
				return Result.Failure<StudentResponse>(StudentErros.StudentNotFound);

			return Result.Success(student.Adapt<StudentResponse>());
		}

		public async Task<Result> DeleteStudentAsync(string id)
		{
			var student = await _userManger.Users.FirstOrDefaultAsync(s => s.Id == id);
			if (student == null)
				return Result.Failure(StudentErros.StudentNotFound);

			var deleteResult = await _userManger.DeleteAsync(student);
			if (!deleteResult.Succeeded)
				return Result.Failure(StudentErros.StudentNotFound);

			return Result.Success();
		}
	}
}
