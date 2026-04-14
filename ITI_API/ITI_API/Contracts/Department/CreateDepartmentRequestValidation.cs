using FluentValidation;

namespace ITI_API.Contracts.Department
{
	public class CreateDepartmentRequestValidation : AbstractValidator<CreateDepartmentRequest>
	{
		public CreateDepartmentRequestValidation()
		{
			RuleFor(d => d.Name)
				.NotEmpty().WithMessage("Department name is required.")
				.MaximumLength(100).WithMessage("Department name must not exceed 100 characters.");
		}
	}
}
