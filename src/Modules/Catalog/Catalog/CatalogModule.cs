using EShop.Catalog.DataSource;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        
        // Api Endpoint services.
        
        // Application UseCase services.
        
        // DataSource - Infrastructure services.
        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<CatalogDbContext>(options => options
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
        );

        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        return app;
    }
}


