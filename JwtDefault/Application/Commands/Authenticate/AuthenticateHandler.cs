using FluentValidation;
using JwtDefault.Application.Common.Helpers;
using JwtDefault.Application.Common.Interfaces;
using LanguageExt.Common;
using MediatR;

namespace JwtDefault.Application.Commands.Authenticate
{
    public class AuthenticateHandler : IRequestHandler<AuthenticateRequest, Result<AuthenticateResponse>>
    {
        private readonly IValidator<AuthenticateRequest> validator;
        private readonly ITokenRepository tokenRepository;
        private readonly IAccountRepository accountRepository;

        public AuthenticateHandler(IValidator<AuthenticateRequest> validator, ITokenRepository tokenRepository, IAccountRepository accountRepository)
        {
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
            this.tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
            this.accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<Result<AuthenticateResponse>> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if (validationResult.IsValid)
            {
                var user = await accountRepository.GetUserByName(request.UserName);
                if (user != null)
                {
                    var userPassword = await accountRepository.GetPassword(user.Id);
                    var encryptedPassword = AuthorizationHelper.EncryptPassword(user.Id, request.UserPassword);
                    if (userPassword == encryptedPassword)
                    {
                        var token = tokenRepository.CreateToken(user);
                        await tokenRepository.CreateRefreshToken(user.Id);
                        return new AuthenticateResponse
                        {
                            Token = token,
                        };
                    }
                }
            }
            return new Result<AuthenticateResponse>(new ValidationException("Bad user or password"));
        }
    }
}
