using ITI_API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITI_API.Persistence.EntitiesConfigrations
{
public class StudentCourseConfigrations : IEntityTypeConfiguration<StudentCourse>
{
public void Configure(EntityTypeBuilder<StudentCourse> builder)
{
builder.HasKey(sc => new { sc.StudentId, sc.CourseId });

builder.Property(sc => sc.Degree)
.HasPrecision(5, 2);

builder.HasOne(sc => sc.Student)
.WithMany(s => s.StudentCourses)
.HasForeignKey(sc => sc.StudentId)
.OnDelete(DeleteBehavior.Cascade);

builder.HasOne(sc => sc.Course)
.WithMany(c => c.StudentCourses)
.HasForeignKey(sc => sc.CourseId)
.OnDelete(DeleteBehavior.Cascade);
}
}
}
