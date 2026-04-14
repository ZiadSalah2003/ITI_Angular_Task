using System.ComponentModel.DataAnnotations;

namespace ITI_API.Authentication
{
	public class JwtOptions
	{
		public static string SectionName = "Jwt";

		[Required]
		public string Key { get; init; } = string.Empty;

		[Range(1, int.MaxValue)]
		public int ExpiryMinutes { get; init; }
	}
}
