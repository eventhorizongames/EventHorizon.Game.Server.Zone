namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Tests;

using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Logging;

using global::System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using NuGet.Common;

using Xunit;

using LogLevel = NuGet.Common.LogLevel;

public class CompilerPackageLoaderNuGetLoggerTests
{
    [Theory]
    [InlineData(LogLevel.Debug)]
    [InlineData(LogLevel.Verbose)]
    [InlineData(LogLevel.Information)]
    [InlineData(LogLevel.Minimal)]
    [InlineData(LogLevel.Warning)]
    [InlineData(LogLevel.Error)]
    public async Task ShouldNotErrorOutWhenLogging(
        LogLevel logLevel
    )
    {
        // Given
        var logMessageMock = new Mock<ILogMessage>();
        logMessageMock.Setup(
            mock => mock.Level
        ).Returns(
            logLevel
        );

        var serviceCollection = new ServiceCollection();

        var loggerMock = new Mock<ILogger<CompilerPackageLoaderNuGetLogger>>();

        serviceCollection.AddSingleton(
            loggerMock.Object
        );

        // When
        var logger = new CompilerPackageLoaderNuGetLogger(
            serviceCollection
        );

        await logger.LogAsync(
            logMessageMock.Object
        );

        // Then
        Assert.True(true);
    }
}
