
using AdminManagementSystem.Models;
using AdminManagementSystem.Repository;
using AdminManagementSystem.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AdminManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddScoped<IStudentRepository,StudentRepository>();
            builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();
            builder.Services.AddScoped<ICourseRepository,CourseRepository>();
            builder.Services.AddScoped<IStudentCourseRepository,StudentCourseRepository>();
            builder.Services.AddScoped<StudentService>();
            builder.Services.AddScoped<CourseService>();
            builder.Services.AddScoped<DepartmentService>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer("Server =.; DataBase = AdminManagementSystem; User Id = sa; Password = 221037; TrustServerCertificate = true");
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false; // No requirement for numeric characters
                options.Password.RequireLowercase = false; // No requirement for lowercase letters
                options.Password.RequireUppercase = false; // No requirement for uppercase letters
                options.Password.RequiredLength = 5; // Minimum length of 1
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0; // No requirement for unique characters
            }).AddEntityFrameworkStores<AppDbContext>();
               

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Acount}/{action=LogIn}/{id?}");

            app.Run();
        }
    }
}
