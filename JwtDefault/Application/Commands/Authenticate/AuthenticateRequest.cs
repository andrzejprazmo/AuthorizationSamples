using LanguageExt.Common;
using MediatR;

namespace JwtDefault.Application.Commands.Authenticate
{
    public class AuthenticateRequest : IRequest<Result<AuthenticateResponse>>
    {
        public string UserName { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
    }
}
