using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using EShop.Basket.DataSource;
using EShop.Catalog.DataSource;

namespace EShop.Api.IntegrationTests.Infrastructure;

public abstract class IntegrationTestBase : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestWebAppFactory Factory;
    protected readonly HttpClient HttpClient;
    protected readonly IServiceScope ServiceScope;

    protected IntegrationTestBase(IntegrationTestWebAppFactory factory)
    {
        Factory = factory;
        HttpClient = factory.CreateClient();
        ServiceScope = factory.Services.CreateScope();
    }

    protected T GetService<T>() where T : notnull
    {
        return ServiceScope.ServiceProvider.GetRequiredService<T>();
    }

    protected T? GetOptionalService<T>() where T : class
    {
        return ServiceScope.ServiceProvider.GetService<T>();
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual Task DisposeAsync()
    {
        HttpClient?.Dispose();
        ServiceScope?.Dispose();
        return Task.CompletedTask;
    }
}

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;
    private readonly RedisContainer _redisContainer;

    public IntegrationTestWebAppFactory()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("eshop_test")
            .WithUsername("test_user")
            .WithPassword("test_password")
            .WithCleanUp(true)
            .Build();

        _redisContainer = new RedisBuilder()
            .WithCleanUp(true)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing database contexts
            services.RemoveAll(typeof(DbContextOptions<BasketDbContext>));
            services.RemoveAll(typeof(DbContextOptions<CatalogDbContext>));

            // Add test database contexts
            services.AddDbContext<BasketDbContext>(options =>
                options.UseNpgsql(_postgresContainer.GetConnectionString()));

            services.AddDbContext<CatalogDbContext>(options =>
                options.UseNpgsql(_postgresContainer.GetConnectionString()));

            // Configure Redis for testing
            services.Configure<Microsoft.Extensions.Caching.StackExchangeRedis.RedisCacheOptions>(options =>
            {
                options.Configuration = _redisContainer.GetConnectionString();
            });

            // Suppress logging during tests for cleaner output
            services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Warning));
        });

        builder.UseEnvironment("Testing");
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        await _redisContainer.StartAsync();
        
        // Ensure databases are created and migrated
        using var scope = Services.CreateScope();
        
        var basketContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
        await basketContext.Database.EnsureCreatedAsync();
        
        var catalogContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        await catalogContext.Database.EnsureCreatedAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
        await _redisContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}