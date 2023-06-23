using JwtCustom.Application.Common.Interfaces;
using JwtCustom.Domain.Account;
using LanguageExt;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace JwtCustom.Infrastructure.Authorization
{
    public class CustomAuthorizationOptions : AuthenticationSchemeOptions
    {
        public const string Scheme = "CustomAuthScheme";
        public const string AccessTokenKey = "X-Access-Token";
        public const string RefreshTokenKey = "X-Refresh-Token";

    }

    public class CustomAuthorizationHandler : AuthenticationHandler<CustomAuthorizationOptions>
    {
        private readonly IConfiguration configuration;
        private readonly ITokenRepository tokenRepository;
        public CustomAuthorizationHandler(IOptionsMonitor<CustomAuthorizationOptions> options
            , ILoggerFactory logger
            , UrlEncoder encoder
            , ISystemClock clock
            , IConfiguration configuration
            , ITokenRepository tokenRepository)
            : base(options, logger, encoder, clock)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Cookies.TryGetValue(CustomAuthorizationOptions.AccessTokenKey, out var token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var tokenParams = new TokenValidationParameters
                    {
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true
                    };
                    var claims = handler.ValidateToken(token, tokenParams, out var tokenSecure);
                    if (tokenSecure.ValidTo < DateTime.UtcNow)
                    {
                        // refresh token
                        var refreshToken = await tokenRepository.GetRefreshToken(token!);
                        if (refreshToken == null) 
                        {
                            return AuthenticateResult.Fail("Cannot find refresh token");
                        }
                        if (refreshToken.Expires < DateTime.UtcNow) 
                        {
                            await tokenRepository.DeleteRefreshToken(refreshToken.UserId);
                            Response.Cookies.Delete(CustomAuthorizationOptions.AccessTokenKey);
                            Response.Cookies.Delete(CustomAuthorizationOptions.RefreshTokenKey);
                            return AuthenticateResult.Fail("Token expired");
                        }
                        await tokenRepository.CreateToken(claims.Claims.ToArray());
                        await tokenRepository.CreateRefreshToken(refreshToken.UserId);
                    }
                    return AuthenticateResult.Success(new AuthenticationTicket(claims, CustomAuthorizationOptions.Scheme));
                }
                catch (Exception e)
                {
                    return AuthenticateResult.Fail(e);
                }
            }
            return AuthenticateResult.Fail("Cannot retrieve token from cookie");
        }
    }
}
