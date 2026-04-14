using ITI_API.Abstractions;
namespace ITI_API.Errors
{
	public static class DepartmentErrors
	{
		public static readonly Error DepartmentNotFound = new("Department.NotFound", "No Department was found with the given id", StatusCodes.Status404NotFound);
		public static readonly Error DepartmentNameAlreadyExists = new("Department.DuplicateName", "A department with the same name already exists", StatusCodes.Status409Conflict);
	}
}
