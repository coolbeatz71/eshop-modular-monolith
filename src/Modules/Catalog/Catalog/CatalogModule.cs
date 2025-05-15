using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using EShop.Catalog.DataSource;
using EShop.Catalog.DataSource.Seed;
using EShop.Shared.Configurations;
using EShop.Shared.DataSource.Extensions;
using EShop.Shared.DataSource.Seed;

namespace EShop.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        
        // Api Endpoint services.
        
        // Application UseCase services.
        
        // DataSource - Infrastructure services.
        var (port, db, user, pass) = AppEnvironment.Database();
        var connectionString = $"Host=127.0.0.1;Port={port};Database={db};Username={user};Password={pass};";
        
        services.AddDbContext<CatalogDbContext>(options => options
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
        );
        
        services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        // Configure Http request pipeline.
        // Use Api endpoint services.
        // Use application UseCase services.
        // Use DataSource - Infrastructure services.
        
        app.UseMigration<CatalogDbContext>();
        return app;
    }
}


