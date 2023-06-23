using Dapper;
using JwtDefault.Application.Common.Interfaces;
using JwtDefault.Domain.Account;
using JwtDefault.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace JwtDefault.Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private const string RefreshTokenKey = "X-Refresh-Token";
        private readonly IDatabaseProvider databaseProvider;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenRepository(IDatabaseProvider databaseProvider, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task CreateRefreshToken(Guid userId)
        {
            var refreshToken = Guid.NewGuid();
            string sql = @"DELETE FROM [dbo].[tokens] WHERE [user_id] = @UserId;
             INSERT INTO [dbo].[tokens]
                   ([token_id]
                   ,[user_id]
                   ,[expires])
             VALUES
                   (@RefreshToken
                   ,@UserId
                   ,@Expires)";
            using (var connection = databaseProvider.GetJwtAuthorizationConnection())
            {
                await connection.ExecuteAsync(sql, new
                {
                    UserId = userId,
                    RefreshToken = refreshToken,
                    Expires = DateTime.UtcNow.AddMinutes(20),
                });
                httpContextAccessor.HttpContext!.Response.Cookies.Append(RefreshTokenKey, refreshToken.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddDays(1),
                });
            }
        }

        public string CreateToken(UserEntity user)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                 }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string? GetCurrentToken()
        {
            if (httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("Authorization", out var headerKey))
            {
                return headerKey.Select(v => v.Replace("Bearer ", string.Empty)).Single();
            }
            return null;

        }

        public async Task DeleteRefreshToken(Guid refreshToken)
        {
            string sql = @"DELETE FROM [dbo].[tokens] WHERE [token_id] = @RefreshToken";
            using (var connection = databaseProvider.GetJwtAuthorizationConnection())
            {
                await connection.ExecuteAsync(sql, new
                {
                    RefreshToken = refreshToken
                });
                httpContextAccessor.HttpContext!.Response.Cookies.Delete(RefreshTokenKey);
            }
        }

        public async Task<RefreshTokenEntity?> GetRefreshToken(string token)
        {
            var refreshTokenId = GetRefreshTokenFromCookies();
            var userId = GetUserIdFromToken(token);
            if (refreshTokenId.HasValue && userId.HasValue)
            {
                string sql = @"SELECT TOP (1) [token_id] AS [Id]
                    ,[user_id] AS [UserId]
                    ,[expires] AS [Expires]
                FROM [dbo].[tokens] WHERE [token_id] = @RefreshToken AND [user_id] = @UserId";
                using (var connection = databaseProvider.GetJwtAuthorizationConnection())
                {
                    return await connection.QuerySingleOrDefaultAsync<RefreshTokenEntity>(sql, new
                    {
                        RefreshToken = refreshTokenId.Value,
                        UserId = userId.Value,
                    });
                }
            }
            return null;
        }

        private Guid? GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            var userIdClaim = claims.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (userIdClaim != null && Guid.TryParse(userIdClaim, out Guid userId))
            {
                return userId;
            }
            return null;
        }

        private Guid? GetRefreshTokenFromCookies()
        {
            if (httpContextAccessor.HttpContext!.Request.Cookies.TryGetValue(RefreshTokenKey, out var cookie))
            {
                if (Guid.TryParse(cookie, out var refreshTokenId))
                {
                    return refreshTokenId;
                }
            }
            return null;
        }
    }
}
