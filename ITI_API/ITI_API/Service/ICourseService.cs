using ITI_API.Abstractions;
using ITI_API.Contracts.Common;
using ITI_API.Contracts.Course;

namespace ITI_API.Service
{
	public interface ICourseService
	{
		Task<Result<PaginatedList<CourseResponse>>> GetAllCoursesAsync(RequestFilters filters);
		Task<Result<CourseResponse>> GetCourseByIdAsync(int id);
		Task<Result<CourseResponse>> GetCourseByNameAsync(string name);
		Task<Result<CourseResponse>> AddCourseAsync(CreateCourseRequest request);
		Task<Result<CourseResponse>> UpdateCourseAsync(int id, UpdateCourseRequest request);
		Task<Result<CourseResponse>> AssignCourseDegreesAsync(AssignCourseDegreesRequest request);
		Task<Result> DeleteCourseAsync(int id);
	}
}
