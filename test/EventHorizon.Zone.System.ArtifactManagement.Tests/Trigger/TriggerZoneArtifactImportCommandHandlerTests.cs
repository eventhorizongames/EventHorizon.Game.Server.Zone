namespace EventHorizon.Zone.System.ArtifactManagement.Tests.Trigger;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System;

using global::System.Threading.Tasks;
using global::System.Threading;
using MediatR;
using Moq;
using Xunit;
using EventHorizon.Zone.System.ArtifactManagement.Tasks;
using EventHorizon.Zone.System.ArtifactManagement.Trigger;
using FluentAssertions;
using EventHorizon.BackgroundTasks.Queue;

public class TriggerZoneArtifactImportCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task EnequeBackgroundJobWhenCommandIsHandled(
        // Given
        string importArticleUrl,
        [Frozen] Mock<ISender> senderMock,
        TriggerZoneArtifactImportCommandHandler handler
    )
    {
        // When
        var actual = await handler.Handle(
            new TriggerZoneArtifactImportCommand(
                importArticleUrl
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();

        senderMock.Verify(
            mock => mock.Send(
                It.Is<EnqueueBackgroundJob>(
                    a =>
                        a.Task.GetType().Equals(typeof(ImportZoneDataTask))
                            && (a.Task as ImportZoneDataTask).ImportArtifactUrl.Equals(importArticleUrl)
                ),
                CancellationToken.None
            )
        );
    }
}
