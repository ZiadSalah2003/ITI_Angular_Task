namespace ITI_API.Contracts.Student
{
	public record UpdateStudentRequest
	(
		string Id,
		string Name,
		int Age,
		int DepartmentId
	);
}
