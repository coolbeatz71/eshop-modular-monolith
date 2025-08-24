using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EShop.TestHelpers.Mocks;

namespace EShop.TestHelpers.Fixtures;

public abstract class BaseTestFixture : IDisposable
{
    protected IServiceProvider ServiceProvider { get; private set; }
    private readonly ServiceCollection _services;
    private bool _disposed = false;

    protected BaseTestFixture()
    {
        _services = new ServiceCollection();
        ConfigureServices(_services);
        ServiceProvider = _services.BuildServiceProvider();
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        // Add logging
        services.AddLogging();
        
        // Add common test services
        services.AddTransient<ILoggerFactory>(provider => MockLogger.CreateFactory().Object);
    }

    protected T GetService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    protected T? GetOptionalService<T>() where T : class
    {
        return ServiceProvider.GetService<T>();
    }

    protected DbContextOptions<TContext> CreateInMemoryDbContextOptions<TContext>(string? databaseName = null) 
        where TContext : DbContext
    {
        return MockDbContext.CreateInMemoryOptions<TContext>(databaseName ?? GetType().Name);
    }

    public virtual void Dispose()
    {
        if (!_disposed)
        {
            if (ServiceProvider is IDisposable disposableProvider)
            {
                disposableProvider.Dispose();
            }
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}

public abstract class DatabaseTestFixture<TContext> : BaseTestFixture 
    where TContext : DbContext
{
    protected TContext Context { get; private set; } = null!;
    protected DbContextOptions<TContext> ContextOptions { get; private set; }

    protected DatabaseTestFixture()
    {
        ContextOptions = CreateInMemoryDbContextOptions<TContext>();
        Context = CreateContext();
        SeedDatabase();
    }

    protected abstract TContext CreateContext();

    protected virtual void SeedDatabase()
    {
        // Override in derived classes to seed test data
    }

    protected async Task<T> ExecuteInNewContextAsync<T>(Func<TContext, Task<T>> operation)
    {
        using var newContext = CreateContext();
        return await operation(newContext);
    }

    protected async Task ExecuteInNewContextAsync(Func<TContext, Task> operation)
    {
        using var newContext = CreateContext();
        await operation(newContext);
    }

    protected T ExecuteInNewContext<T>(Func<TContext, T> operation)
    {
        using var newContext = CreateContext();
        return operation(newContext);
    }

    protected void ExecuteInNewContext(Action<TContext> operation)
    {
        using var newContext = CreateContext();
        operation(newContext);
    }

    public override void Dispose()
    {
        Context?.Dispose();
        base.Dispose();
    }
}