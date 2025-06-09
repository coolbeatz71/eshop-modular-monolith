using System.Reflection;
using EShop.Basket.DataSource;
using EShop.Shared.Behaviors;
using EShop.Shared.Configurations;
using EShop.Shared.DataSource.Interceptors;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        // Api Endpoint services.
        // Application UseCase services.
        // DataSource - Infrastructure services.
        
        // Add MediatR handlers from this assembly.
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(config =>
        {
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        // Read DB connection info from environment.
        var (port, db, user, pass) = AppEnvironment.Database();
        var connectionString = $"Host=127.0.0.1;Port={port};Database={db};Username={user};Password={pass};";
        
        // Register EF Core interceptors.
        services.AddSingleton<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddSingleton<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        
        // Register BasketDbContext with Postgres provider and naming convention.
        services.AddDbContextPool<BasketDbContext>((serviceProvider, options) =>
        {
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });
        
        return services;
    }

    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        // Configure Http request pipeline.
        // Use Api endpoint services.
        // Use application UseCase services.
        // Use DataSource - Infrastructure services.
        
        return app;
    }
}


