using FluentValidation;

namespace JwtCustom.Application.Commands.Authenticate
{
    public class AuthenticateValidator : AbstractValidator<AuthenticateRequest>
    {
        public AuthenticateValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.UserPassword).NotEmpty();
        }
    }
}
