using FluentValidation;

namespace StudentEnrollment.Data.Models.Student;
public class CreateStudentDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public string Identity { get; set; } = default!;
    public byte[] ProfilePicture { get; set; } = default!;
    public string OriginalFileName { get; set; } = default!;
}

public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
{
    public CreateStudentDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();
        RuleFor(x => x.LastName)
            .NotEmpty();
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now)
            .NotEmpty();
        RuleFor(x => x.Identity)
            .NotEmpty();
        RuleFor(x => x.OriginalFileName)
            .NotNull()
            .When(x => x.ProfilePicture != null);
    }
}