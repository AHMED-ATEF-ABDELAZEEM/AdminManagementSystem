using Microsoft.EntityFrameworkCore;

namespace AdminManagementSystem.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department_Course> Departments_Courses { get; set; }
        public DbSet<Student_Course> Students_Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;DataBase=AdminManagementSystem;User Id=sa;Password=221037;TrustServerCertificate=true");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department_Course>()
                .HasKey(x => new {x.Course_Id,x.Department_Id});

            modelBuilder.Entity<Student_Course>()
                .HasKey(x => new { x.Student_Id, x.Course_Id });

            base.OnModelCreating(modelBuilder);
        }
    }
}
