using FluentValidation;

namespace JwtDefault.Installers
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection InstallApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(ApplicationInstaller).Assembly);
            });

            services.AddValidatorsFromAssembly(typeof(ApplicationInstaller).Assembly);

            return services;
        }
    }
}
