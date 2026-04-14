using ITI_API.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITI_API.Persistence.EntitiesConfigrations
{
	public class RoleConfigrations : IEntityTypeConfiguration<IdentityRole>
	{
		public void Configure(EntityTypeBuilder<IdentityRole> builder)
		{
			builder.HasData([
				new IdentityRole
				{
					Id = DefaultRoles.AdminId,
					Name = DefaultRoles.AdminName,
					NormalizedName = DefaultRoles.AdminNormalizedName,
					ConcurrencyStamp = DefaultRoles.AdminConcurrencyStamp,
				},
				new IdentityRole
				{
					Id = DefaultRoles.StudentId,
					Name = DefaultRoles.StudentName,
					NormalizedName = DefaultRoles.StudentNormalizedName,
					ConcurrencyStamp = DefaultRoles.StudentConcurrencyStamp,
				}
			]);
		}
	}
}
