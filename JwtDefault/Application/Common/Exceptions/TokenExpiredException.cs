using FluentValidation;

namespace JwtDefault.Application.Common.Exceptions
{
    public class TokenExpiredException : ValidationException
    {
        public TokenExpiredException(string message) : base(message)
        {
        }
    }
}
