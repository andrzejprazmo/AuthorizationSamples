using Dapper;
using JwtCustom.Application.Common.Interfaces;
using JwtCustom.Domain.Account;
using JwtCustom.Infrastructure.Database;
using JwtCustom.Infrastructure.Database.Entities;

namespace JwtCustom.Infrastructure.Repositories
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

        public async Task<User> GetUserById(Guid userId)
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
                return await connection.QuerySingleAsync<User>(sql, new
                {
                    UserId = userId
                });
            }
        }

        public async Task<User?> GetUserByName(string userName)
        {
            string sql = @"SELECT [users].[user_id]
                  ,[users].[user_name]
                  ,[users].[user_first_name] 
                  ,[users].[user_last_name]
                  ,[users].[user_email_address]
				  ,[roles].role_id
				  ,[roles].role_name
				  ,[roles].role_desc
              FROM [dbo].[users] [users] 
				JOIN [dbo].[user_in_role] [ur] ON [users].[user_id] = [ur].[user_id]
				JOIN [dbo].[roles] [roles] ON [ur].[role_id] = [roles].[role_id]
              WHERE [user_name] = @UserName";
            using (var connection = databaseProvider.GetJwtAuthorizationConnection())
            {
                var list = await connection.QueryAsync<UserRoleEntity>(sql, new
                {
                    UserName = userName
                });
                return list
                    .GroupBy(u => new { u.user_id, u.user_name, u.user_first_name, u.user_last_name, u.user_email_address })
                    .Select(u => new User
                    {
                        Id = u.Key.user_id,
                        FirstName = u.Key.user_first_name,
                        LastName = u.Key.user_last_name,
                        EmailAddress = u.Key.user_email_address,
                        UserName = u.Key.user_name,
                        Roles = u.Select(r => r.role_name).ToArray()
                    }).SingleOrDefault();
            }
        }
    }
}
