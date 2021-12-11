namespace EventHorizon.Zone.System.ArtifactManagement.Tests.Trigger;

using AutoFixture.Xunit2;

using EventHorizon.BackgroundTasks.Queue;
using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.ArtifactManagement.Tasks;
using EventHorizon.Zone.System.ArtifactManagement.Trigger;

using FluentAssertions;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class TriggerZoneArtifactExportCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task EnequeBackgroundJobWhenCommandIsHandled(
        // Given
        [Frozen] Mock<ISender> senderMock,
        TriggerZoneArtifactExportCommandHandler handler
    )
    {
        // When
        var actual = await handler.Handle(
            new TriggerZoneArtifactExportCommand(),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();

        senderMock.Verify(
            mock => mock.Send(
                It.Is<EnqueueBackgroundJob>(
                    a =>
                        a.Task.GetType().Equals(typeof(ExportZoneDataTask))
                ),
                CancellationToken.None
            )
        );
    }
}
