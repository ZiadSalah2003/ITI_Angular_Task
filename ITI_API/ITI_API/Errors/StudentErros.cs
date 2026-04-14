using ITI_API.Abstractions;
namespace ITI_API.Errors
{
    public static class StudentErros
    {
        public static readonly Error StudentNotFound = new("Student.NotFound", "No Student was found with the given id", StatusCodes.Status404NotFound);
        public static readonly Error DepartmentNotFound = new("Student.DepartmentNotFound", "No Department was found with the given id", StatusCodes.Status404NotFound);
    }
}