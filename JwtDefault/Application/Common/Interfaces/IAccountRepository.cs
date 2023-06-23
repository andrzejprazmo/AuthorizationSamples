using JwtDefault.Domain.Account;

namespace JwtDefault.Application.Common.Interfaces
{
    public interface IAccountRepository
    {
        Task<UserEntity> GetUserByName(string userName);
        Task<UserEntity> GetUserById(Guid userId);
        Task<string?> GetPassword(Guid userId);

    }
}
