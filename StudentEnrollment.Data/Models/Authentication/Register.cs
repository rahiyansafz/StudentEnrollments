using FluentValidation;

namespace StudentEnrollment.Data.Models.Authentication;
public class Register : Login
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
}

public class RegisterValidator : AbstractValidator<Register>
{
    public RegisterValidator()
    {
        Include(new LoginValidator());

        RuleFor(x => x.FirstName)
            .NotEmpty();
        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.DateOfBirth)
            .Must((dob) =>
            {
                if (dob.HasValue)
                    return dob.Value < DateTime.Now;

                return true;
            });
    }
}