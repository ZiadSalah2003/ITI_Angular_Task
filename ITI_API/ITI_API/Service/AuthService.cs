using ITI_API.Abstractions;
using ITI_API.Authentication;
using ITI_API.Contracts.Auth;
using ITI_API.Errors;
using ITI_API.Model;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace ITI_API.Service
{
	public class AuthService  : IAuthService
	{
		private readonly UserManager<Student> _userManger;
		private readonly IJwtProvider _jwtProvider;
		public AuthService(UserManager<Student> userManger, IJwtProvider jwtProvider)
		{
			_userManger = userManger;
			_jwtProvider = jwtProvider;
		}
		public async Task<Result<AuthResponse>> Login(LoginRequest request)
		{
			var user = await _userManger.FindByEmailAsync(request.Email);
			if (user is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidCredentails);
			
			var userPassword = await _userManger.CheckPasswordAsync(user, request.Password);
			if (!userPassword)
				return Result.Failure<AuthResponse>(UserErrors.InvalidCredentails);

			var userRoles = await _userManger.GetRolesAsync(user);
			var token = _jwtProvider.GenerateToken(user, userRoles);

			await _userManger.UpdateAsync(user);
			var response = new AuthResponse(token);
			return Result.Success(response);
		}
		public async Task<Result> Register(RegisterRequest request)
		{
			var emailIsExist = await _userManger.Users.AnyAsync(x => x.Email == request.Email);
			if (emailIsExist)
				return Result.Failure(UserErrors.DuplicatedEmail);
			var user = request.Adapt<Student>();
			user.UserName = request.Email;

			var result = await _userManger.CreateAsync(user, request.Password);
			if (result.Succeeded)
			{
				await _userManger.AddToRoleAsync(user, DefaultRoles.AdminName);
				return Result.Success();
			}
			var error = result.Errors.First();
			return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
		}
	}
}
