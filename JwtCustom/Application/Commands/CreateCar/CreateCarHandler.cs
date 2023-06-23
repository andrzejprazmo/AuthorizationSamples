using FluentValidation;
using JwtCustom.Application.Common.Interfaces;
using LanguageExt.Common;
using MediatR;

namespace JwtCustom.Application.Commands.CreateCar
{
    public class CreateCarHandler : IRequestHandler<CreateCarRequest, Result<Guid>>
    {
        private readonly ICarRepository carRepository;
        private readonly IValidator<CreateCarRequest> validator;

        public CreateCarHandler(ICarRepository carRepository, IValidator<CreateCarRequest> validator)
        {
            this.carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<Result<Guid>> Handle(CreateCarRequest request, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if(validationResult.IsValid) 
            {
                return await carRepository.Create(new Domain.Car.Car
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                });
            }
            return new Result<Guid>(new ValidationException(validationResult.Errors));
        }
    }
}
