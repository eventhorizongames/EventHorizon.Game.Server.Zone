namespace EventHorizon.Zone.System.ClientAssets.Tests.Reload;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.System.ClientAssets.Api;
using EventHorizon.Zone.System.ClientAssets.Load;
using EventHorizon.Zone.System.ClientAssets.Reload;

using FluentAssertions;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;
using Xunit.Sdk;

public class ReloadClientAssetsSystemCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ClearAndLoadSystemClientAssetsAndPublishClientActionWhenEventIsHandled(
        // Given
        [Frozen]
            Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        [Frozen]
            Mock<ClientAssetRepository> clientAssetRepositoryMock,
        ReloadClientAssetsSystemCommandHandler handler
    )
    {
        var expected =
            "CLIENT_ASSETS_SYSTEM_RELOADED_CLIENT_ACTION_EVENT";
        // When
        var actual = await handler.Handle(
            new ReloadClientAssetsSystemCommand(),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
        clientAssetRepositoryMock.Verify(
            mock => mock.Clear()
        );
        senderMock.Verify(
            mock =>
                mock.Send(
                    new LoadSystemClientAssetsCommand(),
                    CancellationToken.None
                )
        );
        publisherMock.Verify(
            mock =>
                mock.Publish(
                    It.Is<ClientActionGenericToAllEvent>(
                        a => a.Action == expected
                    ),
                    CancellationToken.None
                )
        );
    }
}
