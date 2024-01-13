namespace EventHorizon.Server.Core.Tests.Register;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Server.Core.Events.Register;
using EventHorizon.Server.Core.Register;
using EventHorizon.Zone.Core.Model.ServerProperty;

using FluentAssertions;

using Moq;

using Xunit;

public class QueryForRegistrationWithCoreServerHandlerTests
{
    [Theory]
    [InlineData("server-id", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public async Task ShouldReturnExpectedWhenBasedOnServerId(
        string serverId,
        bool expected
    )
    {
        // Given
        var serverPropertyMock = new Mock<IServerProperty>();

        serverPropertyMock.Setup(
            mock => mock.Get<string>(
                ServerPropertyKeys.SERVER_ID
            )
        ).Returns(
            serverId
        );

        // When
        var handler = new QueryForRegistrationWithCoreServerHandler(
            serverPropertyMock.Object
        );
        var actual = await handler.Handle(
            new QueryForRegistrationWithCoreServer(),
            CancellationToken.None
        );

        // Then
        actual.Should().Be(expected);
    }
}
