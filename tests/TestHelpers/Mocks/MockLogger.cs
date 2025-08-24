using Microsoft.Extensions.Logging;
using Moq;

namespace EShop.TestHelpers.Mocks;

public static class MockLogger
{
    public static Mock<ILogger<T>> Create<T>()
    {
        var mockLogger = new Mock<ILogger<T>>();
        
        mockLogger.Setup(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception?>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()));

        mockLogger.Setup(x => x.IsEnabled(It.IsAny<LogLevel>()))
                 .Returns(true);

        return mockLogger;
    }

    public static ILogger<T> CreateInstance<T>()
    {
        return Create<T>().Object;
    }

    public static Mock<ILoggerFactory> CreateFactory()
    {
        var mockFactory = new Mock<ILoggerFactory>();
        
        mockFactory.Setup(f => f.CreateLogger(It.IsAny<string>()))
                  .Returns((string categoryName) => 
                  {
                      var loggerMock = new Mock<ILogger>();
                      loggerMock.Setup(x => x.Log(
                          It.IsAny<LogLevel>(),
                          It.IsAny<EventId>(),
                          It.IsAny<It.IsAnyType>(),
                          It.IsAny<Exception?>(),
                          It.IsAny<Func<It.IsAnyType, Exception?, string>>()));
                      return loggerMock.Object;
                  });

        return mockFactory;
    }
}