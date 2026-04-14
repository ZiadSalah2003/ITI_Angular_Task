namespace ITI_API.Contracts.Student
{
	public record StudentResponse(
		string Id,
		string Name,
		int Age,
		string DepartmentName
	);
}
