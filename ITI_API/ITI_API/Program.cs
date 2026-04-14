using FluentValidation;
using FluentValidation.AspNetCore;
using ITI_API.Authentication;
using ITI_API.Mapping;
using ITI_API.Model;
using ITI_API.Persistence;
using ITI_API.Repositories;
using ITI_API.Service;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;

namespace ITI_API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			//builder.Services.AddOpenApi();
			builder.Services.AddEndpointsApiExplorer();

			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));

			builder.Services.AddScoped<ICourseService, CourseService>();
			builder.Services.AddScoped<IStudentService, StudentService>();
			builder.Services.AddScoped<IDepartmentService, DepartmentService>();
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", policy =>
					policy
						.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader()
				);
			});


			builder.Services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			builder.Services.AddIdentity<Student, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

			builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
			builder.Services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName).ValidateDataAnnotations().ValidateOnStart();

			var jwtSettings = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(o =>
				{
					o.SaveToken = true;
					o.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateLifetime = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!))
					};
				});

			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				//app.MapOpenApi();
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseCors("AllowAll");
			app.MapControllers();

			app.Run();
		}
	}
}
