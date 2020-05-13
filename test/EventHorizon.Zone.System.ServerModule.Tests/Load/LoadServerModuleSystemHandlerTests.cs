namespace EventHorizon.Zone.System.ServerModule.Tests.Load
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.ServerModule.Load;
    using EventHorizon.Zone.System.ServerModule.Model;
    using EventHorizon.Zone.System.ServerModule.State;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class LoadServerModuleSystemHandlerTests
    {
        [Fact]
        public async Task ShouldAddAnyFoundServerModuleScriptFilesFoundInServerModuleClientPath()
        {
            // Given
            var expectedServerModuleScripts = new ServerModuleScripts();
            var clientPath = "client-path";
            var serverModuleFilePath = Path.Combine(
                clientPath,
                "ServerModule"
            );
            var file1FullName = "file1-FullName";

            var files = new List<StandardFileInfo>
            {
                new StandardFileInfo(
                    "file1-Name",
                    "file1-Directory",
                    file1FullName,
                    "file1-Extension"
                )
            };

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            var serverModuleRepositoryMock = new Mock<ServerModuleRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetListOfFilesFromDirectory(
                        serverModuleFilePath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                files
            );

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
            );

            fileLoaderMock.Setup(
                mock => mock.GetFile<ServerModuleScripts>(
                    file1FullName
                )
            ).ReturnsAsync(
                expectedServerModuleScripts
            );


            // When
            var handler = new LoadServerModuleSystemHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                fileLoaderMock.Object,
                serverModuleRepositoryMock.Object
            );
            await handler.Handle(
                new LoadServerModuleSystem(),
                CancellationToken.None
            );

            // Then
            serverModuleRepositoryMock.Verify(
                mock => mock.Add(
                    expectedServerModuleScripts
                )
            );
        }
    }
}
