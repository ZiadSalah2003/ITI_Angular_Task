using ITI_API.Model;

namespace ITI_API.Authentication
{
	public interface IJwtProvider
	{
		string GenerateToken(Student user, IEnumerable<string> roles);
		string? ValidateToken(string token);
	}
}
