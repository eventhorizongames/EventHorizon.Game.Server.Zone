namespace EventHorizon.Server.Core.Connection.Tests.Internal;

using System.Threading.Tasks;

using EventHorizon.Server.Core.Connection;
using EventHorizon.Server.Core.Connection.Internal;
using EventHorizon.Server.Core.Connection.Model;

using Moq;

using Xunit;

public class SystemCoreServerConnectionApiTests
{
    [Fact]
    public async Task ShouldCallRegisterZoneSendActionWhenRequestZoneIsCalled()
    {
        // Given
        var expected = new RegisteredZoneDetails();
        var expectedAction = "RegisterZone";
        var request = new ZoneRegistrationDetails();
        var connectionMock = new Mock<CoreServerConnection>();

        // When
        var connectionApi = new SystemCoreServerConnectionApi(
            connectionMock.Object
        );
        var actual = await connectionApi.RegisterZone(
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
    public async Task ShouldCallPingSendActionWhenPingIsCalled()
    {
        // Given
        var expected = "Ping";
        var connectionMock = new Mock<CoreServerConnection>();

        // When
        var connectionApi = new SystemCoreServerConnectionApi(
            connectionMock.Object
        );
        await connectionApi.Ping();

        // Then
        connectionMock.Verify(
            mock => mock.SendAction(
                expected
            )
        );
    }
}
