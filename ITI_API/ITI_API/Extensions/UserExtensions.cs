using System.Security.Claims;

namespace ITI_API.Extensions
{
	public static class UserExtensions
	{
		public static string? GetUserId(this ClaimsPrincipal user)
		{
			return user.FindFirstValue(ClaimTypes.NameIdentifier);
		}
	}
}
