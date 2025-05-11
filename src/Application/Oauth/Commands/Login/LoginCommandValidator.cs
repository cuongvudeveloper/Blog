using Blog.Domain.Constants;

namespace Blog.Application.Oauth.Commands.Login;
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        _ = RuleFor(v => v.Email)
        .EmailAddress()
        .NotEmpty()
        .MaximumLength(DataConfigs.DefaultString);

        _ = RuleFor(v => v.Password)
        .NotEmpty()
        .MinimumLength(DataConfigs.PasswordMinLength)
        .MaximumLength(DataConfigs.DefaultString);
    }
}
