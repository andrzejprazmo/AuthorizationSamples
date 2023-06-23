using JwtDefault.Application.Common.Exceptions;
using JwtDefault.Application.Common.Interfaces;
using LanguageExt.Common;
using MediatR;

namespace JwtDefault.Application.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, Result<RefreshTokenResponse>>
    {
        private readonly IAccountRepository accountRepository;
        private readonly ITokenRepository tokenRepository;
        public RefreshTokenHandler(IAccountRepository accountRepository, ITokenRepository tokenRepository)
        {
            this.accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            this.tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        }


        public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var accessToken = tokenRepository.GetCurrentToken();
            if (accessToken != null)
            {
                var refreshToken = await tokenRepository.GetRefreshToken(accessToken);
                if (refreshToken != null)
                {
                    if (refreshToken.Expires > DateTime.UtcNow)
                    {
                        var user = await accountRepository.GetUserById(refreshToken.UserId);
                        if (user != null)
                        {
                            var newAccessToken = tokenRepository.CreateToken(user);
                            await tokenRepository.CreateRefreshToken(user.Id);
                            return new RefreshTokenResponse
                            {
                                Token = newAccessToken,
                            };
                        }
                    }
                    await tokenRepository.DeleteRefreshToken(refreshToken.Id);
                    return new Result<RefreshTokenResponse>(new TokenExpiredException("Token expired"));
                }
            }
            return new Result<RefreshTokenResponse>(new Exception("Cannot retrieve token"));
        }
    }
}
