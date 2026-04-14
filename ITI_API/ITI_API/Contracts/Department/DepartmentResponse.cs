namespace ITI_API.Contracts.Department
{
	public record DepartmentResponse
	(
		int Id,
		string Name,
		int StudentsCount,
		List<string> StudentNames
	);
}
