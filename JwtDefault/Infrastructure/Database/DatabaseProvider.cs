using System.Data.SqlClient;

namespace JwtDefault.Infrastructure.Database
{
    public interface IDatabaseProvider
    {
        SqlConnection GetJwtAuthorizationConnection();
    }
    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly IConfiguration configuration;

        public DatabaseProvider(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public SqlConnection GetJwtAuthorizationConnection()
        {
            return new SqlConnection(configuration.GetConnectionString("JwtAuthorization"));
        }
    }
}
