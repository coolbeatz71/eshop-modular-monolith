using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace EShop.TestHelpers.Mocks;

public static class MockDbContext
{
    public static Mock<TContext> Create<TContext>() where TContext : DbContext
    {
        var mockContext = new Mock<TContext>();
        
        // Mock SaveChanges methods
        mockContext.Setup(c => c.SaveChanges()).Returns(1);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(1);

        return mockContext;
    }

    public static Mock<DbSet<TEntity>> CreateDbSet<TEntity>(List<TEntity>? data = null) 
        where TEntity : class
    {
        data ??= new List<TEntity>();
        
        var queryableData = data.AsQueryable();
        var mockSet = new Mock<DbSet<TEntity>>();

        mockSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        mockSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        mockSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        mockSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

        mockSet.Setup(d => d.Add(It.IsAny<TEntity>())).Callback<TEntity>(data.Add);
        mockSet.Setup(d => d.Remove(It.IsAny<TEntity>())).Callback<TEntity>(entity => data.Remove(entity));

        return mockSet;
    }

    public static DbContextOptions<TContext> CreateInMemoryOptions<TContext>(string? databaseName = null) 
        where TContext : DbContext
    {
        databaseName ??= Guid.NewGuid().ToString();
        
        return new DbContextOptionsBuilder<TContext>()
            .UseInMemoryDatabase(databaseName)
            .EnableSensitiveDataLogging()
            .Options;
    }
}