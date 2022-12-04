using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StudentEnrollment.Data.Entities;

namespace StudentEnrollment.Data.Data.Configurations;
internal class CourseConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasData(
            new Course()
            {
                Id = 1,
                Title = "Minimal API Development",
                Credits = 7
            },
            new Course()
            {
                Id = 2,
                Title = "Web API Development",
                Credits = 7
            },
            new Course()
            {
                Id = 3,
                Title = "Azure Function Development",
                Credits = 5
            },
            new Course()
            {
                Id = 4,
                Title = "Micro Service Development",
                Credits = 6
            }
        );
    }
}
