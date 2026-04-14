using ITI_API.Abstractions;
using ITI_API.Contracts.Common;
using ITI_API.Contracts.Student;

namespace ITI_API.Service
{
	public interface IStudentService
	{
		Task<Result<PaginatedList<StudentResponse>>> GetAllStudentsAsync(RequestFilters filters);
		Task<Result<StudentResponse>> GetStudentByIdAsync(string? studentId, string id);
		Task<Result<StudentResponse>> CreateStudentAsync(CreateStudentRequest request);
		Task<Result<StudentResponse>> UpdateStudentAsync(string id, UpdateStudentRequest request);
		Task<Result> DeleteStudentAsync(string id);
	}
}
