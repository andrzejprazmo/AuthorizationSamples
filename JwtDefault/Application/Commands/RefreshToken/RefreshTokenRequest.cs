using LanguageExt.Common;
using MediatR;

namespace JwtDefault.Application.Commands.RefreshToken
{
    public record RefreshTokenRequest : IRequest<Result<RefreshTokenResponse>>;
}
