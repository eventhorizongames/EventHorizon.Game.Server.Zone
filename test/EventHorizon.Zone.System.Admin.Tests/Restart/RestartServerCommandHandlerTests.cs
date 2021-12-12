namespace EventHorizon.Zone.System.Admin.Tests.Restart;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Restart;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;

using FluentAssertions;

using global::System;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;
using Xunit.Sdk;

public class RestartServerCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task MyTestMethod(
        // Given
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        RestartServerCommandHandler handler
    )
    {
        // When
        var actual = await handler.Handle(
            new RestartServerCommand(),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();

        senderMock.Verify(
            mock => mock.Send(
                new RunServerStartupCommand(),
                CancellationToken.None
            )
        );
        senderMock.Verify(
            mock => mock.Send(
                new FinishServerStartCommand(),
                CancellationToken.None
            )
        );

        publisherMock.Verify(
            mock => mock.Publish(
                It.Is<AdminCommandEvent>(
                    a => a.Command.Command == "reload-admin"
                ),
                CancellationToken.None
            )
        );
        publisherMock.Verify(
            mock => mock.Publish(
                It.Is<AdminCommandEvent>(
                    a => a.Command.Command == "reload-system"
                ),
                CancellationToken.None
            )
        );
    }
    
   [Theory, AutoMoqData]
    public async Task ReturnFailedResultWhenExecptionIsThrown(
        // Given
        Exception exception,
        [Frozen] Mock<ISender> senderMock,
        RestartServerCommandHandler handler
    )
    {
        var expected = "SERVER_RESTART_ERROR";

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<RunServerStartupCommand>(),
                CancellationToken.None
            )
        ).ThrowsAsync(
            exception
        );

        // When
        var actual = await handler.Handle(
            new RestartServerCommand(),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }
}
