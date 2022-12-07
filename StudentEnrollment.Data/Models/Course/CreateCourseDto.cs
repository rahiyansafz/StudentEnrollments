using FluentValidation;

namespace StudentEnrollment.Data.Models.Course;

public class CreateCourseDto
{
    public string Title { get; set; } = default!;
    public int Credits { get; set; }
}

public class CreateCourseValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();
        RuleFor(x => x.Credits)
            .GreaterThan(0);
    }
}