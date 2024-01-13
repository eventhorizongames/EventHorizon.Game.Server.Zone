namespace EventHorizon.Server.Core.Tests.Disconnected;

using System.Threading;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using EventHorizon.Server.Core.Connection.Disconnected;
using EventHorizon.Server.Core.Disconnected;
using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Model.ServerProperty;

using Moq;

using Xunit;

public class CoreServerConnectionDisconnectedHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldNullServerIdWhenRequestIsHandled(
        // Given
        [Frozen] Mock<IServerProperty> serverPropertyMock,
        CoreServerConnectionDisconnectedHandler handler
    )
    {
        // When
        await handler.Handle(
            new ServerCoreConnectionDisconnected(),
            CancellationToken.None
        );

        // Then
        serverPropertyMock.Verify(
            mock => mock.Remove(
                ServerPropertyKeys.SERVER_ID
            )
        );
    }
}
