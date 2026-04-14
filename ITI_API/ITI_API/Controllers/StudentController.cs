using ITI_API.Abstractions;
using ITI_API.Contracts.Auth;
using ITI_API.Contracts.Common;
using ITI_API.Contracts.Student;
using ITI_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentController : ControllerBase
	{
		private readonly IStudentService _studentService;
		private readonly IAuthService _authService;

		public StudentController(IStudentService studentService, IAuthService authService)
		{
			_studentService = studentService;
			_authService = authService;
		}
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters)
		{
			var students = await _studentService.GetAllStudentsAsync(filters);
			return Ok(students);
		}
		[Authorize(Roles = "Admin")]
		[HttpGet("{id}")]
		[Authorize(Roles =DefaultRoles.StudentName)]
		public async Task<IActionResult> GetById(string id)
		{
			var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var result = await _studentService.GetStudentByIdAsync(studentId, id);
			if (result.IsFailure)
				return result.ToProblem();
			return Ok(result.Value);
		}

		//[HttpPost]
		//public async Task<IActionResult> Create([FromBody] CreateStudentRequest request)
		//{
		//	var result = await _studentService.CreateStudentAsync(request);
		//	if (result.IsFailure)
		//		return result.ToProblem();
		//	return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
		//}
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request)
		{
			var result = await _authService.Register(request);
			if (result.IsFailure)
				return result.ToProblem();

			return Ok();
		}
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var authResult = await _authService.Login(request);
			return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
		}
		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(string id, [FromBody] UpdateStudentRequest request)
		{
			if (id != request.Id)
				return BadRequest();

			var result = await _studentService.UpdateStudentAsync(id, request);
			if (result.IsFailure)
				return result.ToProblem();

			return Ok(result.Value);
		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _studentService.DeleteStudentAsync(id);
			if (result.IsFailure)
				return result.ToProblem();

			return NoContent();
		}
	}
}
