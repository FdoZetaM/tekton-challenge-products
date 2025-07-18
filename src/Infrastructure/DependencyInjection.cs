namespace TektonChallengeProducts.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Services;
using Domain.Abstractions.Persistence;
using Domain.Abstractions.Services;
using Infrastructure.Persistence.Sql;
using Infrastructure.Persistence.Sql.Repositories;
using Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddCacheService();

        services.AddDbContext<EntityFrameworkUnitOfWork>(
            options => options.UseSqlServer(configuration.GetConnectionString("SqlConnection")!)
        );

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<EntityFrameworkUnitOfWork>());
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddHttpClient();
        services.AddScoped<IDiscountService, DiscountService>(provider =>
        {
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new DiscountService(httpClientFactory, configuration);
        });

        return services;
    }

    private static void AddCacheService(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();
    }
}
