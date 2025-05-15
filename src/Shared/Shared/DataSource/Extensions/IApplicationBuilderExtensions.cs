using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using EShop.Shared.DataSource.Seed;

namespace EShop.Shared.DataSource.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app) 
        where TContext : DbContext
    {
        MigrateDatabaseAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();
        SeedDatabaseAsync(app.ApplicationServices).GetAwaiter().GetResult();
        
        return app;
    }

    private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider serviceProvider)
        where TContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        
        await context.Database.MigrateAsync();
    }
    
    private static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        await Task.WhenAll(seeders.Select(seeder => seeder.SeedAllAsync()));
    }
}