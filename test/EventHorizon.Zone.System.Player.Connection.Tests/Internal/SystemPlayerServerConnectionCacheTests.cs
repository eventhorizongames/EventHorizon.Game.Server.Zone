using System;
using System.Net.Http;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Player.Connection.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Player.Connection.Tests.Internal
{
    /// <summary>
    /// Since this class is a Wrapper around a SignalR Connection we cannot create a valid
    ///  connection to the Player Server without having an up and running Player Server.
    /// So to these Tests will only test for Errors/Exceptions.
    /// Since the internal connection will always be null, the calls are just to make sure 
    ///  they pass expectedly.
    /// </summary>
    public class SystemPlayerServerConnectionCacheTests
    {
        [Fact]
        public async Task TestShouldReturnConnection()
        {
            // Given
            var url = "http://player_server_url";

            var loggerMock = new Mock<ILogger<SystemPlayerServerConnectionCache>>();

            // When
            var connectionCache = new SystemPlayerServerConnectionCache(
                loggerMock.Object
            );

            Func<Task> connectionAction = async () => await connectionCache.GetConnection(
                url,
                options => { }
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
            var loggerMock = new Mock<ILogger<SystemPlayerServerConnectionCache>>();

            // When
            var connectionCache = new SystemPlayerServerConnectionCache(
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
            var loggerMock = new Mock<ILogger<SystemPlayerServerConnectionCache>>();

            // When
            var connectionCache = new SystemPlayerServerConnectionCache(
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