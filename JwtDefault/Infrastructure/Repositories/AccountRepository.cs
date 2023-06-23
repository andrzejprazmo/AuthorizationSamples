using Dapper;
using JwtDefault.Application.Common.Interfaces;
using JwtDefault.Domain.Account;
using JwtDefault.Infrastructure.Database;

namespace JwtDefault.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDatabaseProvider databaseProvider;

        public AccountRepository(IDatabaseProvider databaseProvider)
        {
            this.databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
        }

        public async Task<string?> GetPassword(Guid userId)
        {
            string sql = @"SELECT TOP (1) [password] FROM [dbo].[passwords] WHERE [active] = 1 AND [user_id] = @UserId";
            using (var connection = databaseProvider.GetJwtAuthorizationConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<string>(sql, new
                {
                    UserId = userId
                });
            }
        }

        public async Task<UserEntity> GetUserById(Guid userId)
        {
            string sql = @"SELECT TOP (1) [user_id] AS [Id]
                  ,[user_name] AS [UserName]
                  ,[user_first_name] AS [FirstName]
                  ,[user_last_name] AS [LastName]
                  ,[user_email_address] AS [EmailAddress]
              FROM [dbo].[users]
              WHERE [user_id] = @UserId";
            using (var connection = databaseProvider.GetJwtAuthorizationConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<UserEntity>(sql, new
                {
                    UserId = userId
                });
            }
        }

        public async Task<UserEntity> GetUserByName(string userName)
        {
            string sql = @"SELECT TOP (1) [user_id] AS [Id]
                  ,[user_name] AS [UserName]
                  ,[user_first_name] AS [FirstName]
                  ,[user_last_name] AS [LastName]
                  ,[user_email_address] AS [EmailAddress]
              FROM [dbo].[users]
              WHERE [user_name] = @UserName";
            using (var connection = databaseProvider.GetJwtAuthorizationConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<UserEntity>(sql, new
                {
                    UserName = userName
                });
            }
        }
    }
}
