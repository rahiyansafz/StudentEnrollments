using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using StudentEnrollment.Data.Data.Configurations;
using StudentEnrollment.Data.Entities;

namespace StudentEnrollment.Data.Data;
public class DataContext : IdentityDbContext<SchoolUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new CourseConfig());
        builder.ApplyConfiguration(new RoleConfig());
        builder.ApplyConfiguration(new SchoolUserConfig());
        builder.ApplyConfiguration(new UserRoleConfig());
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Student> Students { get; set; }
}