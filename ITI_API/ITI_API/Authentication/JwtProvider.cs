using ITI_API.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ITI_API.Authentication
{
	public class JwtProvider : IJwtProvider
	{
		private readonly JwtOptions _jwtOptions;
		public JwtProvider(IOptions<JwtOptions> jwtOptions)
		{
			_jwtOptions = jwtOptions.Value;
		}
		public string GenerateToken(Student user, IEnumerable<string> roles)
		{
			Claim[] claims = new Claim[]
			{
				new(JwtRegisteredClaimNames.Sub,user.Id),
				new(JwtRegisteredClaimNames.Email,user.Email!),
				new(nameof(roles),JsonSerializer.Serialize(roles),JsonClaimValueTypes.JsonArray)
			};
			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
			var singingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
				signingCredentials: singingCredentials
				);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public string? ValidateToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					IssuerSigningKey = symmetricSecurityKey,
					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);
				var jwtToken = (JwtSecurityToken)validatedToken;
				return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
			}
			catch
			{
				return null;
			}
		}
	}
}
