using JwtCustom.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtCustom.Installers
{
    public static class CustomAuthInstaller
    {
        public static IServiceCollection InstallCustomAuthorization(this IServiceCollection services)
        {
            services
                .AddAuthentication(options => options.DefaultScheme = CustomAuthorizationOptions.Scheme)
                .AddScheme<CustomAuthorizationOptions, CustomAuthorizationHandler>(CustomAuthorizationOptions.Scheme, options =>
                {
                });

            return services;
        }
    }
}
