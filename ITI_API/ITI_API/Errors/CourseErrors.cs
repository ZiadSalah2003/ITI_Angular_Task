using ITI_API.Abstractions;

namespace ITI_API.Errors
{
	public static class CourseErrors
	{
		public static readonly Error CourseNotFound = new("Course.NotFound", "No Course was found with the given id", StatusCodes.Status404NotFound);
		public static readonly Error CourseNameAlreadyExists = new("Course.DuplicateName", "A course with the same name already exists", StatusCodes.Status409Conflict);
		public static readonly Error DepartmentNotFound = new("Course.DepartmentNotFound", "No Department was found with the given id", StatusCodes.Status404NotFound);
		public static readonly Error CourseDepartmentMismatch = new("Course.DepartmentMismatch", "The course does not belong to the given department", StatusCodes.Status400BadRequest);
		public static readonly Error InvalidStudentsForDepartment = new("Course.InvalidStudentsForDepartment", "One or more students are not found or do not belong to the given department", StatusCodes.Status400BadRequest);
	}
}