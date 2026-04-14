using ITI_API.Abstractions;
using ITI_API.Contracts.Common;
using ITI_API.Contracts.Department;
using ITI_API.Contracts.Student;
using ITI_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class DepartmentController : ControllerBase
	{
		private readonly IDepartmentService _departmentService;

		public DepartmentController(IDepartmentService departmentService)
		{
			_departmentService = departmentService;
		}

		[HttpGet]
		[EndpointDescription("Get all departments with optional pagination and filtering.")]
		[EndpointSummary("Get All Departments")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentResponse))]
		public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters)
		{
			var departments = await _departmentService.GetAllDepartmentsAsync(filters);
			return Ok(departments);
		}

		[HttpGet("{id}")]
		[EndpointDescription("get depratment by id")]
		[EndpointSummary("Get Department by id")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DepartmentResponse))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(void))]
		public async Task<IActionResult> GetById(int id)
		{
			var result = await _departmentService.GetDepartmentByIdAsync(id);
			if (result.IsFailure)
				return result.ToProblem();
			return Ok(result.Value);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request)
		{
			var result = await _departmentService.CreateDepartmentAsync(request);
			if (result.IsFailure)
				return result.ToProblem();
			return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdateDepartmentRequest request)
		{
			if (id != request.Id)
				return BadRequest();

			var result = await _departmentService.UpdateDepartmentAsync(id, request);
			if (result.IsFailure)
				return result.ToProblem();

			return Ok(result.Value);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _departmentService.DeleteDepartmentAsync(id);
			if (result.IsFailure)
				return result.ToProblem();

			return NoContent();
		}
	}
}
