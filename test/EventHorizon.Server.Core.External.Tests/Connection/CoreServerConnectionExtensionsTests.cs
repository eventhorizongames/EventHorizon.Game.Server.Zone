using System.Threading.Tasks;
using EventHorizon.Server.Core.External.Connection;
using EventHorizon.Server.Core.External.Connection.Model;
using Moq;
using Xunit;

namespace EventHorizon.Server.Core.External.Tests.Connection
{
    public class CoreServerConnectionExtensionsTests
    {
        [Fact]
        public async Task TestShouldCallRegisterZoneSendActionWhenRequestZoneIsCalled()
        {
            // Given
            var expected = new RegisteredZoneDetails();
            var expectedAction = "RegisterZone";
            var request = new ZoneRegistrationDetails();
            var connectionMock = new Mock<CoreServerConnection>();

            // When
            var actual = await CoreServerConnectionExtensions.RegisterZone(
                connectionMock.Object,
                request
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
            connectionMock.Verify(
                mock => mock.SendAction<RegisteredZoneDetails>(
                    expectedAction,
                    request
                )
            );
        }
        [Fact]
        public async Task TestShouldCallPingSendActionWhenPingIsCalled()
        {
            // Given
            var expected = "Ping";
            var connectionMock = new Mock<CoreServerConnection>();

            // When
            await CoreServerConnectionExtensions.Ping(
                connectionMock.Object
            );

            // Then
            connectionMock.Verify(
                mock => mock.SendAction(
                    expected
                )
            );
        }
    }
}