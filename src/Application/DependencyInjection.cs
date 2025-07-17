namespace TektonChallengeProducts.Application;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(assembly);

        return services;
    }

    private static void AddMediatR(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));

        var applicationAssembly = Assembly.Load("TektonChallengeProducts.Application");
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(applicationAssembly);
            config.RegisterServicesFromAssembly(assembly);
        });
    }
}
