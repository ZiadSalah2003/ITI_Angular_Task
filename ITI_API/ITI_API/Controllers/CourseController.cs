using ITI_API.Abstractions;
using ITI_API.Contracts.Common;
using ITI_API.Contracts.Course;
using ITI_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles ="Admin")]
	public class CourseController : ControllerBase
	{
		private readonly ICourseService _courseService;

		public CourseController(ICourseService courseService)
		{
			_courseService = courseService;
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] RequestFilters filters)
		{
			var courses = await _courseService.GetAllCoursesAsync(filters);
			return Ok(courses);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _courseService.DeleteCourseAsync(id);
			if (result.IsFailure)
				return result.ToProblem();

			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCourseRequest request)
		{
			if (id != request.Id)
				return BadRequest();

			var result = await _courseService.UpdateCourseAsync(id, request);
			if (result.IsFailure)
				return result.ToProblem();

			return Ok(result.Value);
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromBody] CreateCourseRequest request)
		{
			if (request == null)
				return BadRequest();

			var result = await _courseService.AddCourseAsync(request);
			if (result.IsFailure)
				return result.ToProblem();

			return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
		}

		[HttpPost("assign-degrees")]
		public async Task<IActionResult> AssignDegrees([FromBody] AssignCourseDegreesRequest request)
		{
			if (request == null)
				return BadRequest();

			var result = await _courseService.AssignCourseDegreesAsync(request);
			if (result.IsFailure)
				return result.ToProblem();

			return Ok(result.Value);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var result = await _courseService.GetCourseByIdAsync(id);
			if (result.IsFailure)
				return result.ToProblem();

			return Ok(result.Value);
		}

		[HttpGet("name/{name}")]
		public async Task<IActionResult> GetByName(string name)
		{
			var result = await _courseService.GetCourseByNameAsync(name);
			if (result.IsFailure)
				return result.ToProblem();

			return Ok(result.Value);
		}
	}
}
