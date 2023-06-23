using LanguageExt.Common;
using MediatR;

namespace JwtCustom.Application.Commands.CreateCar
{
    public record CreateCarRequest(string Name) : IRequest<Result<Guid>>;
}
