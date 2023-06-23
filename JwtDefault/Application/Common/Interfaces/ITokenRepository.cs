using JwtDefault.Domain.Account;

namespace JwtDefault.Application.Common.Interfaces
{
    public interface ITokenRepository
    {
        string CreateToken(UserEntity user);
        Task CreateRefreshToken(Guid userId);
        Task<RefreshTokenEntity?> GetRefreshToken(string token);
        Task DeleteRefreshToken(Guid refreshToken);
        string? GetCurrentToken();
    }
}
