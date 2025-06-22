using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using EShop.Catalog.DataSource;
using EShop.Catalog.DataSource.Seed;
using EShop.Shared.Behaviors;
using EShop.Shared.Configurations;
using EShop.Shared.DataSource.Extensions;
using EShop.Shared.DataSource.Seed;
using EShop.Shared.DataSource.Interceptors;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EShop.Catalog;

/// <summary>
/// Provides extension methods to register and configure the Catalog module's services and middleware.
/// </summary>
public static class CatalogModule
{
    /// <summary>
    /// Adds the Catalog module's services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    /// <param name="configuration">The application configuration instance.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
    /// <remarks>
    /// Registers MediatR handlers, database context with interceptors, and a data seeder.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.Services.AddCatalogModule(builder.Configuration);
    /// </code>
    /// </example>
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        // Api Endpoint services.
        // Application UseCase services.
        // DataSource - Infrastructure services.
        
        // Read DB connection info from environment.
        var (port, db, user, pass) = AppEnvironment.Database();
        var connectionString = $"Host=127.0.0.1;Port={port};Database={db};Username={user};Password={pass};";

        // Register EF Core interceptors.
        services.AddSingleton<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddSingleton<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        // Register CatalogDbContext with Postgres provider and naming convention.
        services.AddDbContextPool<CatalogDbContext>((serviceProvider, options) =>
        {
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        // Register data seeder for initial data population.
        services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        return services;
    }

    /// <summary>
    /// Configures the Catalog module's middleware in the application pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The updated <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <remarks>
    /// Applies pending EF Core migrations and executes the data seeder.
    /// </remarks>
    /// <example>
    /// <code>
    /// app.UseCatalogModule();
    /// </code>
    /// </example>
    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        // Configure Http request pipeline.
        // Use Api endpoint services.
        // Use application UseCase services.
        // Use DataSource - Infrastructure services.
        
        app.UseMigration<CatalogDbContext>();
        app.UseSeed();

        return app;
    }
}
