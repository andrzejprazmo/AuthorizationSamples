using JwtCustom.Domain.Account;

namespace JwtCustom.Application.Common.Interfaces
{
    public interface IAccountRepository
    {
        Task<User?> GetUserByName(string userName);
        Task<User> GetUserById(Guid userId);
        Task<string?> GetPassword(Guid userId);

    }
}
