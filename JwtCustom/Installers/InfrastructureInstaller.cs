using JwtCustom.Application.Common.Interfaces;
using JwtCustom.Infrastructure.Database;
using JwtCustom.Infrastructure.Repositories;

namespace JwtCustom.Installers
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection InstallInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseProvider, DatabaseProvider>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<ICarRepository, CarRepository>();

            return services;
        }
    }
}
