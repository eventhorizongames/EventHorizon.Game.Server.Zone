using System;
using System.Net.Http;
using System.Threading.Tasks;
using EventHorizon.Server.Core.External.Connection.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventHorizon.Server.Core.External.Tests.Connection.Internal
{
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
        public async Task TestShouldReturnConnection()
        {
            // Given
            var url = "http://core_server_url";

            var loggerMock = new Mock<ILogger<SystemCoreServerConnectionCache>>();

            // When
            var connectionCache = new SystemCoreServerConnectionCache(
                loggerMock.Object
            );

            Func<Task> connectionAction = async () => await connectionCache.GetConnection(
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
        public void TestShouldPassWhenDisposeIsCalled()
        {
            // Given
            var loggerMock = new Mock<ILogger<SystemCoreServerConnectionCache>>();

            // When
            var connectionCache = new SystemCoreServerConnectionCache(
                loggerMock.Object
            );
            connectionCache.Dispose();

            // Then
            Assert.True(
                true
            );
        }

        [Fact]
        public async Task TestShouldPassWhenStopIsCalled()
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
}