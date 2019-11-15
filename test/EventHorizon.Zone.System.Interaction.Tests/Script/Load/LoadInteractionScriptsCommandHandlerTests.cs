using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Interaction.Script.Load;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Interaction.Tests.Script.Load
{
    public class LoadInteractionScriptsCommandHandlerTests
    {
        [Fact]
        public async Task TestShouldRegisterScriptsFromServerScriptsPath()
        {
            // Given
            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile = null;
            IDictionary<string, object> arguments = null;
            var serverScriptsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "server-scripts-path"
            );
            var interactionDirectoryFullName = Path.Combine(
                serverScriptsPath,
                "Interaction"
            );
            var interactionFileName = "LoadedScript.csx";
            var interactionFileFullName = Path.Combine(
                interactionDirectoryFullName,
                interactionFileName
            );
            var interactionFileContent = "// Script Comment";
            var fileExtension = ".exe";
            var interactionFileInfo = new StandardFileInfo(
                interactionFileName,
                interactionDirectoryFullName,
                interactionFileFullName,
                fileExtension
            );

            var expectedTestData1 = new TestData(
                interactionFileName,
                "Interaction",
                interactionFileContent
            );
            var expectedReferenceAssemblies = typeof(LoadInteractionScriptsCommandHandler).Assembly;

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<LoadFileRecursivelyFromDirectory>(),
                    CancellationToken.None
                )
            ).Callback<IRequest<Unit>, CancellationToken>(
                (evt, token) =>
                {
                    onProcessFile = ((LoadFileRecursivelyFromDirectory)evt).OnProcessFile;
                    arguments = ((LoadFileRecursivelyFromDirectory)evt).Arguments;
                }
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextFromFile(
                        interactionFileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                interactionFileContent
            );

            // When
            var handler = new LoadInteractionScriptsCommandHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );

            await handler.Handle(
                new LoadInteractionScriptsCommand(),
                CancellationToken.None
            );
            Assert.NotNull(
                onProcessFile
            );

            await onProcessFile(
                interactionFileInfo,
                arguments
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<RegisterServerScriptCommand>(
                        command => command.FileName == expectedTestData1.FileName
                            && command.Path == expectedTestData1.FilePath
                            && command.ScriptString == expectedTestData1.FileContent
                    ),
                    CancellationToken.None
                )
            );
        }

        struct TestData
        {
            public string FileName { get; }
            public string FilePath { get; }
            public string FileContent { get; }

            public TestData(
                string fileName,
                string filePath,
                string fileContent
            )
            {
                this.FileName = fileName;
                this.FilePath = filePath;
                this.FileContent = fileContent;
            }
        }
    }
}