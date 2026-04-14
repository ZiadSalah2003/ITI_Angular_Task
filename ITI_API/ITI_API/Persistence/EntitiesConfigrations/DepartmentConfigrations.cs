using ITI_API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITI_API.Persistence.EntitiesConfigrations
{
	public class DepartmentConfigrations : IEntityTypeConfiguration<Department>
	{
		public void Configure(EntityTypeBuilder<Department> builder)
		{
			builder.HasIndex(d => d.Name)
				.IsUnique()
				.HasFilter("[IsDeleted] = 0");

			builder.HasMany(d => d.Students)
					.WithOne(s => s.Department)
					.HasForeignKey(s => s.DepartmentId)
					.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(d => d.Courses)
					.WithOne(c => c.Department)
					.HasForeignKey(c => c.DepartmentId)
					.OnDelete(DeleteBehavior.SetNull);

		}
	}
}
