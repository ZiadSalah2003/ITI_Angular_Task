using ITI_API.Abstractions;

namespace ITI_API.Errors
{
	public static class UserErrors
	{
		public static readonly Error InvalidCredentails = new("User.InvalidCredentials", "Invalid Email Or Password", StatusCodes.Status401Unauthorized);
		public static readonly Error DuplicatedEmail = new("User.DuplicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);
		public static readonly Error UserNotFound = new("User.UserNotFound", "User is not found", StatusCodes.Status404NotFound);
	}
}
