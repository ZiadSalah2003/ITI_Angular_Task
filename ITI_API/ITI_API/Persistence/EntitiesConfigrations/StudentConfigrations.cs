using ITI_API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITI_API.Persistence.EntitiesConfigrations
{
	public class StudentConfigrations : IEntityTypeConfiguration<Student>
	{
		public void Configure(EntityTypeBuilder<Student> builder)
		{
			builder.HasOne(s => s.Department)
					.WithMany(d => d.Students)
					.HasForeignKey(s => s.DepartmentId)
					.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
