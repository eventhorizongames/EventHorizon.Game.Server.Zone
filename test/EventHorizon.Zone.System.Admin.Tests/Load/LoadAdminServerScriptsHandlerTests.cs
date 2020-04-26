using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Admin.Load;
using EventHorizon.Zone.System.Server.Scripts.Events.Load;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Admin.Tests.Load
{
    public class LoadAdminServerScriptsHandlerTests
    {
        [Fact]
        public async Task TestName()
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

            var expectedInteractionFileName = interactionFileName;
            var expectedInteractionPath = "Interaction";
            var expectedInteractionFileContent = interactionFileContent;

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var systemProvidedAssemblyListMock = new Mock<SystemProvidedAssemblyList>();

            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ProcessFilesRecursivelyFromDirectory>(),
                    CancellationToken.None
                )
            ).Callback<IRequest<Unit>, CancellationToken>(
                (evt, token) =>
                {
                    onProcessFile = ((ProcessFilesRecursivelyFromDirectory)evt).OnProcessFile;
                    arguments = ((ProcessFilesRecursivelyFromDirectory)evt).Arguments;
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
            var handler = new LoadAdminServerScriptsHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                systemProvidedAssemblyListMock.Object
            );

            await handler.Handle(
                new LoadServerScriptsCommand(),
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
                        command => command.FileName == expectedInteractionFileName
                            && command.Path == expectedInteractionPath
                            && command.ScriptString == expectedInteractionFileContent
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}