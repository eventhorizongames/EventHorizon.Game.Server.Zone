namespace EventHorizon.Zone.System.ClientEntities.Tests.Load;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.ClientEntities.Load;
using EventHorizon.Zone.System.ClientEntities.Model;
using EventHorizon.Zone.System.ClientEntities.Query;
using EventHorizon.Zone.System.ClientEntities.Register;
using EventHorizon.Zone.System.ClientEntities.Unregister;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class LoadSystemClientEntitiesCommandHandlerTests
{
    [Fact]
    public async Task ShouldRegisterClientEntityLoadedFromClientEntityPath()
    {
        // Given
        var clientEntityPath = "client-entity-path";
        var fileInfoList = new List<StandardFileInfo>();
        for (int i = 0; i < 10; i++)
        {
            fileInfoList.Add(
                new StandardFileInfo(
                    $"file-{i}",
                    $"file-{i}-director",
                    $"file-{i}-full-name",
                    "json"
                )
            );
        }

        var mediatorMock = new Mock<IMediator>();
        var fileLoaderMock = new Mock<IJsonFileLoader>();
        var serverInfoMock = new Mock<ServerInfo>();

        mediatorMock.Setup(
            mock => mock.Send(
                new GetListOfFilesFromDirectory(
                    clientEntityPath
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            fileInfoList
        );

        fileLoaderMock.Setup(
            mock => mock.GetFile<ClientEntityDetails>(
                It.IsAny<string>()
            )
        ).ReturnsAsync(
            (string path) => new ClientEntityDetails
            {
                Name = path,
                Data = new ConcurrentDictionary<string, object>()
            }
        );

        serverInfoMock.Setup(
            mock => mock.ClientEntityPath
        ).Returns(
            clientEntityPath
        );

        // When
        var handler = new LoadSystemClientEntitiesCommandHandler(
            mediatorMock.Object,
            fileLoaderMock.Object,
            serverInfoMock.Object
        );
        await handler.Handle(
            new LoadSystemClientEntitiesCommand(),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                It.IsAny<RegisterClientEntityCommand>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Exactly(10)
        );
    }

    [Fact]
    public async Task ShouldUnregisterClientEntityWhenExistingEntitiesAreFound()
    {
        // Given
        var existingClientEntityList = new List<IObjectEntity>();
        for (int i = 0; i < 10; i++)
        {
            existingClientEntityList.Add(
                new ClientEntity(
                    $"client-entity-{i}",
                    new ConcurrentDictionary<string, object>()
                )
            );
        }

        var mediatorMock = new Mock<IMediator>();
        var fileLoaderMock = new Mock<IJsonFileLoader>();
        var serverInfoMock = new Mock<ServerInfo>();

        mediatorMock.Setup(
            mock => mock.Send(
                new QueryForAllClientEntityDetailsList(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            existingClientEntityList
        );

        // When
        var handler = new LoadSystemClientEntitiesCommandHandler(
            mediatorMock.Object,
            fileLoaderMock.Object,
            serverInfoMock.Object
        );
        await handler.Handle(
            new LoadSystemClientEntitiesCommand(),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                It.IsAny<UnregisterClientEntity>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Exactly(10)
        );
    }
}
