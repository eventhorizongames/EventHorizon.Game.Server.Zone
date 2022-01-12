namespace EventHorizon.Zone.System.ClientAssets.Tests.Update;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.ClientAssets.Api;
using EventHorizon.Zone.System.ClientAssets.Events.Update;
using EventHorizon.Zone.System.ClientAssets.Save;
using EventHorizon.Zone.System.ClientAssets.Update;

using FluentAssertions;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class UpdateClientAssetCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShoudlSetClientAssetInRepositoryWhenCommandIsHandled(
        // Given
        UpdateClientAssetCommand request,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen]
            Mock<ClientAssetRepository> clientAssetRepositoryMock,
        UpdateClientAssetCommandHandler handler
    )
    {
        // When
        var actual = await handler.Handle(
            request,
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();

        mediatorMock.Verify(
            mock =>
                mock.Publish(
                    new RunSaveClientAssetsEvent(),
                    CancellationToken.None
                )
        );
        clientAssetRepositoryMock.Verify(
            mock => mock.Set(request.ClientAsset)
        );
    }
}
