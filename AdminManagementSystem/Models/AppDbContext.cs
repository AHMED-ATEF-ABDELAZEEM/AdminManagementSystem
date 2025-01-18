using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Models
{

    public class AppDbContext : IdentityDbContext<ApplicationUser>
        {

        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Department_Course> Departments_Courses { get; set; }
		public DbSet<Student_Course> Students_Courses { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Department_Course>()
				.HasKey(x => new { x.Course_Id, x.Department_Id });

			modelBuilder.Entity<Student_Course>()
				.HasKey(x => new { x.Student_Id, x.Course_Id });

			base.OnModelCreating(modelBuilder);
		}
	}
}
