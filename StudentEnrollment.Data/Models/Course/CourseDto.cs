using FluentValidation;

namespace StudentEnrollment.Data.Models.Course;
public class CourseDto : CreateCourseDto
{
    public int Id { get; set; }
}

public class CourseValidator : AbstractValidator<CourseDto>
{
    public CourseValidator()
    {
        Include(new CreateCourseValidator());
    }
}