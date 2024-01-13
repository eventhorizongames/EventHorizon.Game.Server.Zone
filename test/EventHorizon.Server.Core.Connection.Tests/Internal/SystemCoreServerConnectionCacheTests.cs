namespace EventHorizon.Server.Core.Connection.Tests.Internal;

using System.Net.Http;
using System.Threading.Tasks;

using EventHorizon.Server.Core.Connection.Internal;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

/// <summary>
/// Since this class is a Wrapper around a SignalR Connection we cannot create a valid
///  connection to the Core Server without having an up and running Core Server.
/// So to these Tests will only test for Errors/Exceptions.
/// Since the internal connection will always be null, the calls are just to make sure 
///  they pass expectedly.
/// </summary>
public class SystemCoreServerConnectionCacheTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task ShouldReturnConnection()
    {
        // Given
        var url = "http://core_server_url";

        var loggerMock = new Mock<ILogger<SystemCoreServerConnectionCache>>();

        // When
        var connectionCache = new SystemCoreServerConnectionCache(
            loggerMock.Object
        );

        async Task connectionAction() => await connectionCache.GetConnection(
            url,
            options => { },
            (ex) => Task.CompletedTask
        );

        // Then
        await Assert.ThrowsAsync<HttpRequestException>(
            connectionAction
        );
    }

    [Fact]
    public async Task ShouldPassWhenDisposeIsCalled()
    {
        // Given
        var loggerMock = new Mock<ILogger<SystemCoreServerConnectionCache>>();

        // When
        var connectionCache = new SystemCoreServerConnectionCache(
            loggerMock.Object
        );
        await connectionCache.DisposeAsync();

        // Then
        Assert.True(
            true
        );
    }

    [Fact]
    public async Task ShouldPassWhenStopIsCalled()
    {
        // Given
        var loggerMock = new Mock<ILogger<SystemCoreServerConnectionCache>>();

        // When
        var connectionCache = new SystemCoreServerConnectionCache(
            loggerMock.Object
        );
        await connectionCache.Stop();

        // Then
        Assert.True(
            true
        );
    }
}
