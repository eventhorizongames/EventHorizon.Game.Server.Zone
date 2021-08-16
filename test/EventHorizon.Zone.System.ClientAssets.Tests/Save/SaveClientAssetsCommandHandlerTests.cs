namespace EventHorizon.Zone.System.ClientAssets.Tests.Save
{
    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.ClientAssets.Model;
    using EventHorizon.Zone.System.ClientAssets.Save;
    using EventHorizon.Zone.System.ClientAssets.State.Api;

    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;

    public class SaveClientAssetsCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldSaveAllClientAssetsWithFileSaverWhenRepositioryHasClientAssets(
            // Given
            List<ClientAsset> clientAssets,
            [Frozen] Mock<IJsonFileSaver> fileSaverMock,
            [Frozen] Mock<ClientAssetRepository> clientAssetRepositoryMock,
            SaveClientAssetsCommandHandler handler
        )
        {
            clientAssetRepositoryMock.Setup(
                mock => mock.All()
            ).Returns(
                clientAssets
            );

            // When
            var actual = await handler.Handle(
                new SaveClientAssetsCommand(),
                CancellationToken.None
            );

            // Then
            foreach (var expected in clientAssets)
            {
                fileSaverMock.Verify(
                    mock => mock.SaveToFile(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        expected
                    )
                );
            }
        }
    }
}
