using EShop.TestHelpers.Extensions;
using EShop.TestHelpers.Fixtures;
using EShop.TestHelpers.Mocks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace EShop.TestHelpers;

public abstract class TestBase : BaseTestFixture
{
    protected static Mock<ILogger<T>> CreateMockLogger<T>() => MockLogger.Create<T>();
    protected static Mock<IMediator> CreateMockMediator() => MockMediatR.CreateMediator();
    
    protected Mock<ILogger<T>> GetMockLogger<T>() => CreateMockLogger<T>();
    protected Mock<IMediator> GetMockMediator() => CreateMockMediator();
}

public abstract class UnitTestBase : TestBase
{
    protected UnitTestBase()
    {
        // Unit tests should be fast and isolated
        // No heavy dependencies should be configured here
    }
}

public abstract class IntegrationTestBase<TContext> : DatabaseTestFixture<TContext>
    where TContext : Microsoft.EntityFrameworkCore.DbContext
{
    protected IntegrationTestBase() : base()
    {
        // Integration tests can have heavier setup
    }
}