using JwtCustom.Domain.Account;
using System.Security.Claims;

namespace JwtCustom.Application.Common.Interfaces
{
    public interface ITokenRepository
    {
        Task CreateToken(User user);
        Task CreateToken(Claim[] claims);
        Task CreateRefreshToken(Guid userId);
        Task<RefreshToken?> GetRefreshToken(string token);
        Task DeleteRefreshToken(Guid refreshToken);
    }
}
