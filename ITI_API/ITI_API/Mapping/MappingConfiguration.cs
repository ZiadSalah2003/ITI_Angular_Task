using ITI_API.Contracts.Student;
using ITI_API.Model;
using Mapster;

namespace ITI_API.Mapping
{
	public class MappingConfiguration : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<CreateStudentRequest, Student>();

			config.NewConfig<UpdateStudentRequest, Student>();

			config.NewConfig<Student, StudentResponse>()
				.Map(dest => dest.DepartmentName, src => src.Department.Name);
		}
	}
}
