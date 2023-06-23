using Dapper;
using JwtCustom.Application.Common.Interfaces;
using JwtCustom.Domain.Car;
using JwtCustom.Infrastructure.Database;

namespace JwtCustom.Infrastructure.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly IDatabaseProvider databaseProvider;

        public CarRepository(IDatabaseProvider databaseProvider)
        {
            this.databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
        }

        public async Task<Guid> Create(Car car)
        {
            string sql = @"INSERT INTO [dbo].[cars] ([car_id], [car_name])
                VALUES (@Id, @Name)";
            using (var connection = databaseProvider.GetJwtAuthorizationConnection())
            {
                await connection.ExecuteAsync(sql, car);
                return car.Id;
            }
        }

        public async Task<IEnumerable<Car>> GetList()
        {
            string sql = @"SELECT TOP (1000) [car_id] AS [Id], [car_name] AS [Name]  FROM [dbo].[cars]";

            using (var connection = databaseProvider.GetJwtAuthorizationConnection())
            {
                return await connection.QueryAsync<Car>(sql);
            }
        }
    }
}
