using FluentValidation;

namespace ITI_API.Contracts.Department
{
	public class UpdateDepartmentRequestValidation : AbstractValidator<UpdateDepartmentRequest>
	{
		public UpdateDepartmentRequestValidation()
		{
			RuleFor(d => d.Id)
				.NotEqual(0).WithMessage("Id musn't be 0 for Updated departments.");

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Department name is required.")
				.MaximumLength(100).WithMessage("Department name must not exceed 100 characters.");
		}
	}
}
