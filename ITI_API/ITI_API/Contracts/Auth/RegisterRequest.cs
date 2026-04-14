namespace ITI_API.Contracts.Auth
{
	public record RegisterRequest
	(
		string Name,
		string Email,
		string Password,
		int Age,
		int? DepartmentId
	);
}
