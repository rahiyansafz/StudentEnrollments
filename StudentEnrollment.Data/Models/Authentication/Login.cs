using FluentValidation;

namespace StudentEnrollment.Data.Models.Authentication;
public class Login
{
    public string EmailAddress { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class LoginValidator : AbstractValidator<Login>
{
    public LoginValidator()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20);
    }
}