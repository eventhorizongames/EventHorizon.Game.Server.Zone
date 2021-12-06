namespace EventHorizon.Zone.Core.Tests.Lifetime;

using System.Threading;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Lifetime.NewFolder;
using EventHorizon.Zone.Core.Lifetime.State;

using FluentAssertions;

using Moq;

using Xunit;

public class PauseServerCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task SetServerStartedToFalseWhenCommandIsHandled(
        // Given
        PauseServerCommand command,
        [Frozen] Mock<ServerLifetimeState> serverLifetimeState,
        PauseServerCommandHandler handler
    )
    {
        // When
        var actual = await handler.Handle(
            command,
            CancellationToken.None
        );

        // Then
        actual.Success.Should()
            .BeTrue();
        serverLifetimeState.Verify(
            mock => mock.SetServerStarted(
                false
            )
        );
    }
}
