namespace ITI_API.Contracts.Student
{
	public record CreateStudentRequest
	(
		string Name,
		int Age,
		int? DepartmentId
	);
}
