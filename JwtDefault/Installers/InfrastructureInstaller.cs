using JwtDefault.Application.Common.Interfaces;
using JwtDefault.Infrastructure.Database;
using JwtDefault.Infrastructure.Repositories;

namespace JwtDefault.Installers
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection InstallInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseProvider, DatabaseProvider>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();

            return services;
        }
    }
}
