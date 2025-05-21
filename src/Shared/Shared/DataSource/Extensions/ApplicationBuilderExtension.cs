using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using EShop.Shared.DataSource.Seed;

namespace EShop.Shared.DataSource.Extensions;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/> to enable database migration and data seeding.
/// </summary>
public static class ApplicationBuilderExtension
{
    /// <summary>
    /// Applies any pending EF Core migrations for the specified DbContext type synchronously during application startup.
    /// </summary>
    /// <typeparam name="TContext">The DbContext type to migrate.</typeparam>
    /// <param name="app">The application builder instance.</param>
    /// <returns>The same <see cref="IApplicationBuilder"/> instance for chaining.</returns>
    /// <example>
    /// <code>
    /// app.UseMigration&lt;CatalogDbContext&gt;();
    /// </code>
    /// </example>
    public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app) 
        where TContext : DbContext
    {
        MigrateDatabaseAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();
        return app;
    }
    
    /// <summary>
    /// Executes all registered data seeders implementing <see cref="IDataSeeder"/> asynchronously during application startup.
    /// </summary>
    /// <param name="app">The application builder instance.</param>
    /// <returns>The same <see cref="IApplicationBuilder"/> instance for chaining.</returns>
    /// <example>
    /// <code>
    /// app.UseSeed();
    /// </code>
    /// </example>
    public static IApplicationBuilder UseSeed(this IApplicationBuilder app)
    {
        SeedDataAsync(app.ApplicationServices).GetAwaiter().GetResult();
        return app;
    }

    /// <summary>
    /// Asynchronously migrates the database for the specified <see cref="DbContext"/>.
    /// </summary>
    /// <typeparam name="TContext">The <see cref="DbContext"/> type to migrate.</typeparam>
    /// <param name="serviceProvider">The service provider to resolve the context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider serviceProvider)
        where TContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        
        await context.Database.MigrateAsync();
    }
    
    /// <summary>
    /// Asynchronously executes all registered data seeders.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve the seeders.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        await Task.WhenAll(seeders.Select(seeder => seeder.SeedAllAsync()));
    }
}
