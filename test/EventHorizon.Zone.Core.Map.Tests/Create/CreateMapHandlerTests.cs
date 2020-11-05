namespace EventHorizon.Zone.Core.Map.Tests.Create
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Performance;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Events.Map.Create;
    using EventHorizon.Zone.Core.Events.Map.Generate;
    using EventHorizon.Zone.Core.Map.Create;
    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Map.State;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.Core.Model.Map;
    using FluentAssertions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class CreateMapHandlerTests
    {
        [Fact]
        public async Task ShouldSetupServerMapWithDetailsFromZoneMapAndGeneratedMap()
        {
            // Given
            var coreMapPath = "core-map-path";
            var mapStateFile = Path.Combine(
                coreMapPath,
                "Map.state.json"
            );
            var zoneMapMesh = new ZoneMapMesh();
            var zoneMapDetails = new ZoneMapDetails
            {
                Mesh = zoneMapMesh,
            };
            var generatedMapGraph = new Mock<IMapGraph>().Object;

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            var serverMapMock = new Mock<IServerMap>();
            var performanceTrackerFactoryMock = new Mock<PerformanceTrackerFactory>();

            serverInfoMock.Setup(
                mock => mock.CoreMapPath
            ).Returns(
                coreMapPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesFileExist(
                        mapStateFile
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GenerateMapFromDetails(
                        zoneMapDetails
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                generatedMapGraph
            );

            fileLoaderMock.Setup(
                mock => mock.GetFile<ZoneMapDetails>(
                    mapStateFile
                )
            ).ReturnsAsync(
                zoneMapDetails
            );

            // When
            var handler = new CreateMapHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                fileLoaderMock.Object,
                serverMapMock.Object,
                performanceTrackerFactoryMock.Object
            );
            await handler.Handle(
                new CreateMap(),
                CancellationToken.None
            );

            // Then
            serverMapMock.Verify(
                mock => mock.SetMap(generatedMapGraph)
            );
            serverMapMock.Verify(
                mock => mock.SetMapDetails(zoneMapDetails)
            );
            serverMapMock.Verify(
                mock => mock.SetMapMesh(zoneMapMesh)
            );
        }

        [Fact]
        public async Task ShouldPublishMapCreatedEventWhenFinishedProcessing()
        {
            // Given
            var expected = new MapCreatedEvent();
            var coreMapPath = "app-data-path";
            var mapStateFile = Path.Combine(
                coreMapPath,
                "Map.state.json"
            );
            var zoneMapMesh = new ZoneMapMesh();
            var zoneMapDetails = new ZoneMapDetails
            {
                Mesh = zoneMapMesh,
            };
            var generatedMapGraph = new Mock<IMapGraph>().Object;

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            var serverMapMock = new Mock<IServerMap>();
            var performanceTrackerFactoryMock = new Mock<PerformanceTrackerFactory>();

            serverInfoMock.Setup(
                mock => mock.CoreMapPath
            ).Returns(
                coreMapPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesFileExist(
                        mapStateFile
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GenerateMapFromDetails(
                        zoneMapDetails
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                generatedMapGraph
            );

            fileLoaderMock.Setup(
                mock => mock.GetFile<ZoneMapDetails>(
                    mapStateFile
                )
            ).ReturnsAsync(
                zoneMapDetails
            );

            // When
            var handler = new CreateMapHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                fileLoaderMock.Object,
                serverMapMock.Object,
                performanceTrackerFactoryMock.Object
            );
            await handler.Handle(
                new CreateMap(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }
}
