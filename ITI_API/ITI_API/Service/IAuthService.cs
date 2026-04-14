using ITI_API.Abstractions;
using ITI_API.Contracts.Auth;

namespace ITI_API.Service
{
	public interface IAuthService
	{
		Task<Result<AuthResponse>> Login(LoginRequest request);
		Task<Result> Register(RegisterRequest request);
	}
}
