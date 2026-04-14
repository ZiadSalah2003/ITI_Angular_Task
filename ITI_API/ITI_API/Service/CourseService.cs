using ITI_API.Abstractions;
using ITI_API.Contracts.Common;
using ITI_API.Contracts.Course;
using ITI_API.Errors;
using ITI_API.Model;
using ITI_API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ITI_API.Service
{
	public class CourseService : ICourseService
	{
		private readonly IUnitOfWork _unitOfWork;

		public CourseService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PaginatedList<CourseResponse>>> GetAllCoursesAsync(RequestFilters filters)
		{
			var courses = _unitOfWork.Courses.GetQueryable()
				.Where(c => !c.IsDeleted)
				.Select(c => new CourseResponse
				{
					Id = c.Id,
					Crs_Name = c.Crs_Name,
					Crs_Description = c.Crs_Description,
					Duration = c.Duration,
					DepartmentId = c.DepartmentId,
					DepartmentName = c.Department != null ? c.Department.Name : null,
					StudentDegrees = new List<StudentDegreeResponse>()
				});

			var response = await PaginatedList<CourseResponse>.CreateAsync(courses, filters.PageNumber, filters.PageSize);
			return Result.Success(response);
		}

		public async Task<Result<CourseResponse>> GetCourseByIdAsync(int id)
		{
			var course = await _unitOfWork.Courses.GetByIdAsync(id);
			if (course == null || course.IsDeleted)
				return Result.Failure<CourseResponse>(CourseErrors.CourseNotFound);

			return Result.Success(new CourseResponse
			{
				Id = course.Id,
				Crs_Name = course.Crs_Name,
				Crs_Description = course.Crs_Description,
				Duration = course.Duration,
				DepartmentId = course.DepartmentId,
				DepartmentName = course.Department?.Name,
				StudentDegrees = course.StudentCourses
					.Select(sc => new StudentDegreeResponse
					{
						StudentId = sc.StudentId,
						StudentName = sc.Student?.Name,
						Degree = sc.Degree
					})
					.ToList()
			});
		}

		public async Task<Result<CourseResponse>> GetCourseByNameAsync(string name)
		{
			var course = await _unitOfWork.Courses.GetQueryable()
				.FirstOrDefaultAsync(c => !c.IsDeleted && c.Crs_Name == name);

			if (course == null)
				return Result.Failure<CourseResponse>(CourseErrors.CourseNotFound);

			return Result.Success(new CourseResponse
			{
				Id = course.Id,
				Crs_Name = course.Crs_Name,
				Crs_Description = course.Crs_Description,
				Duration = course.Duration,
				DepartmentId = course.DepartmentId,
				DepartmentName = course.Department?.Name,
				StudentDegrees = course.StudentCourses
					.Select(sc => new StudentDegreeResponse
					{
						StudentId = sc.StudentId,
						StudentName = sc.Student?.Name,
						Degree = sc.Degree
					})
					.ToList()
			});
		}

		public async Task<Result<CourseResponse>> AddCourseAsync(CreateCourseRequest request)
		{
			var courseNameExists = await _unitOfWork.Courses.GetQueryable()
				.AnyAsync(c => !c.IsDeleted && c.Crs_Name == request.Crs_Name);

			if (courseNameExists)
				return Result.Failure<CourseResponse>(CourseErrors.CourseNameAlreadyExists);

			if (request.DepartmentId.HasValue)
			{
				var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId.Value);
				if (department == null || department.IsDeleted)
					return Result.Failure<CourseResponse>(CourseErrors.DepartmentNotFound);
			}

			var newCourse = new Course
			{
				Crs_Name = request.Crs_Name,
				Crs_Description = request.Crs_Description,
				Duration = request.Duration,
				DepartmentId = request.DepartmentId
			};

			await _unitOfWork.Courses.AddAsync(newCourse);
			await _unitOfWork.SaveAsync();

			if (request.StudentDegrees != null && request.StudentDegrees.Count > 0)
			{
				var distinctStudentDegrees = request.StudentDegrees
					.Where(sd => !string.IsNullOrWhiteSpace(sd.StudentId))
					.GroupBy(sd => sd.StudentId)
					.Select(group => group.Last())
					.ToList();

				foreach (var studentDegree in distinctStudentDegrees)
				{
					newCourse.StudentCourses.Add(new StudentCourse
					{
						StudentId = studentDegree.StudentId,
						CourseId = newCourse.Id,
						Degree = studentDegree.Degree
					});
				}

				await _unitOfWork.SaveAsync();
			}

			return Result.Success(new CourseResponse
			{
				Id = newCourse.Id,
				Crs_Name = newCourse.Crs_Name,
				Crs_Description = newCourse.Crs_Description,
				Duration = newCourse.Duration,
				DepartmentId = newCourse.DepartmentId,
				DepartmentName = newCourse.Department?.Name,
				StudentDegrees = newCourse.StudentCourses
					.Select(sc => new StudentDegreeResponse
					{
						StudentId = sc.StudentId,
						StudentName = sc.Student?.Name,
						Degree = sc.Degree
					})
					.ToList()
			});
		}

		public async Task<Result<CourseResponse>> UpdateCourseAsync(int id, UpdateCourseRequest request)
		{
			var course = await _unitOfWork.Courses.GetByIdAsync(id);
			if (course == null || course.IsDeleted)
				return Result.Failure<CourseResponse>(CourseErrors.CourseNotFound);

			var courseNameExists = await _unitOfWork.Courses.GetQueryable()
				.AnyAsync(c => c.Id != id && !c.IsDeleted && c.Crs_Name == request.Crs_Name);

			if (courseNameExists)
				return Result.Failure<CourseResponse>(CourseErrors.CourseNameAlreadyExists);

			if (request.DepartmentId.HasValue)
			{
				var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId.Value);
				if (department == null || department.IsDeleted)
					return Result.Failure<CourseResponse>(CourseErrors.DepartmentNotFound);
			}

			course.Crs_Name = request.Crs_Name;
			course.Crs_Description = request.Crs_Description;
			course.Duration = request.Duration;
			course.DepartmentId = request.DepartmentId;

			course.StudentCourses.Clear();

			if (request.StudentDegrees != null && request.StudentDegrees.Count > 0)
			{
				var distinctStudentDegrees = request.StudentDegrees
					.Where(sd => !string.IsNullOrWhiteSpace(sd.StudentId))
					.GroupBy(sd => sd.StudentId)
					.Select(group => group.Last())
					.ToList();

				foreach (var studentDegree in distinctStudentDegrees)
				{
					course.StudentCourses.Add(new StudentCourse
					{
						StudentId = studentDegree.StudentId,
						CourseId = course.Id,
						Degree = studentDegree.Degree
					});
				}
			}

			_unitOfWork.Courses.Update(course);
			await _unitOfWork.SaveAsync();

			return Result.Success(new CourseResponse
			{
				Id = course.Id,
				Crs_Name = course.Crs_Name,
				Crs_Description = course.Crs_Description,
				Duration = course.Duration,
				DepartmentId = course.DepartmentId,
				DepartmentName = course.Department?.Name,
				StudentDegrees = course.StudentCourses
					.Select(sc => new StudentDegreeResponse
					{
						StudentId = sc.StudentId,
						StudentName = sc.Student?.Name,
						Degree = sc.Degree
					})
					.ToList()
			});
		}

		public async Task<Result<CourseResponse>> AssignCourseDegreesAsync(AssignCourseDegreesRequest request)
		{
			var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
			if (department == null || department.IsDeleted)
				return Result.Failure<CourseResponse>(CourseErrors.DepartmentNotFound);

			var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
			if (course == null || course.IsDeleted)
				return Result.Failure<CourseResponse>(CourseErrors.CourseNotFound);

			if (course.DepartmentId != request.DepartmentId)
				return Result.Failure<CourseResponse>(CourseErrors.CourseDepartmentMismatch);

			var distinctStudentDegrees = (request.StudentDegrees ?? new List<StudentDegreeRequest>())
				.Where(sd => !string.IsNullOrWhiteSpace(sd.StudentId))
				.GroupBy(sd => sd.StudentId)
				.Select(group => group.Last())
				.ToList();

			var studentIds = distinctStudentDegrees.Select(sd => sd.StudentId).ToList();
			if (studentIds.Count > 0)
			{
				var validStudentIds = await _unitOfWork.Students.GetQueryable()
					.Where(s => studentIds.Contains(s.Id) && !s.IsDeleted && s.DepartmentId == request.DepartmentId)
					.Select(s => s.Id)
					.ToListAsync();

				if (validStudentIds.Count != studentIds.Count)
					return Result.Failure<CourseResponse>(CourseErrors.InvalidStudentsForDepartment);
			}

			course.StudentCourses.Clear();

			foreach (var studentDegree in distinctStudentDegrees)
			{
				course.StudentCourses.Add(new StudentCourse
				{
					StudentId = studentDegree.StudentId,
					CourseId = course.Id,
					Degree = studentDegree.Degree
				});
			}

			_unitOfWork.Courses.Update(course);
			await _unitOfWork.SaveAsync();

			return Result.Success(new CourseResponse
			{
				Id = course.Id,
				Crs_Name = course.Crs_Name,
				Crs_Description = course.Crs_Description,
				Duration = course.Duration,
				DepartmentId = course.DepartmentId,
				DepartmentName = course.Department?.Name,
				StudentDegrees = course.StudentCourses
					.Select(sc => new StudentDegreeResponse
					{
						StudentId = sc.StudentId,
						StudentName = sc.Student?.Name,
						Degree = sc.Degree
					})
					.ToList()
			});
		}

		public async Task<Result> DeleteCourseAsync(int id)
		{
			var course = await _unitOfWork.Courses.GetByIdAsync(id);
			if (course == null || course.IsDeleted)
				return Result.Failure(CourseErrors.CourseNotFound);

			course.IsDeleted = true;
			_unitOfWork.Courses.Update(course);
			await _unitOfWork.SaveAsync();
			return Result.Success();
		}
	}
}
