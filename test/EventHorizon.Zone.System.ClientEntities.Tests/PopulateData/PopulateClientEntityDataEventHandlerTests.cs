namespace EventHorizon.Zone.System.ClientEntities.Tests.PopulateData
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.PopulateData;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Xunit;

    public class PopulateClientEntityDataEventHandlerTests
    {
        [Fact]
        public async Task ShouldPopulateClientEntityWhenEventIsHandled()
        {
            // Given
            var expectedAssetId = "asset-id";
            var expectedDense = true;
            var expectedDensityBox = Vector3.One;
            var expectedResolveHeight = true;
            var expectedheightOffset = 10L;

            var clientEntity = new ClientEntity(
                "client-entity",
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        { "assetId", expectedAssetId },
                        { "dense", expectedDense },
                        { "densityBox", expectedDensityBox },
                        { "resolveHeight", expectedResolveHeight },
                        { "heightOffset", expectedheightOffset }
                    }
                )
            );

            // When
            var handler = new PopulateClientEntityDataEventHandler();
            await handler.Handle(
                new PopulateClientEntityDataEvent(
                    clientEntity
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                clientEntity.GetProperty<string>(
                    nameof(ClientEntityMetadataTypes.assetId)
                ),
                expectedAssetId
            );
            Assert.Equal(
                clientEntity.GetProperty<bool>(
                    nameof(ClientEntityMetadataTypes.dense)
                ),
                expectedDense
            );
            Assert.Equal(
                clientEntity.GetProperty<Vector3>(
                    nameof(ClientEntityMetadataTypes.densityBox)
                ),
                expectedDensityBox
            );
            Assert.Equal(
                clientEntity.GetProperty<bool>(
                    nameof(ClientEntityMetadataTypes.resolveHeight)
                ),
                expectedResolveHeight
            );
            Assert.Equal(
                clientEntity.GetProperty<long>(
                    nameof(ClientEntityMetadataTypes.heightOffset)
                ),
                expectedheightOffset
            );
        }
    }
}