using FluentValidation;

namespace JwtCustom.Application.Commands.CreateCar
{
    public class CreateCarValidator : AbstractValidator<CreateCarRequest>
    {
        public CreateCarValidator()
        {
            RuleFor(x=>x.Name).NotEmpty();
        }
    }
}
