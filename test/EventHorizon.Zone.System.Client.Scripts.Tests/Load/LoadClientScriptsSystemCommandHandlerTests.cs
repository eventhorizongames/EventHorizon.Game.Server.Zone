namespace EventHorizon.Zone.System.Client.Scripts.Tests.Load
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Load;
    using EventHorizon.Zone.System.Client.Scripts.Model;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class LoadClientScriptsSystemCommandHandlerTests
    {
        [Fact]
        public async Task MyTestMethod()
        {
            // Given
            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile = null;
            IDictionary<string, object> arguments = null;
            var clientScriptsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "client-scripts-path"
            );

            var javaScriptFileName = "JavaScript.js";
            var javaScriptDirectoryFullName = Path.Combine(
                clientScriptsPath,
                "Actions"
            );
            var javaScriptFileFullName = "javascript-file-full-name";
            var javaScriptFileExtension = ".js";
            var javaScriptFileInfo = new StandardFileInfo(
                javaScriptFileName,
                javaScriptDirectoryFullName,
                javaScriptFileFullName,
                javaScriptFileExtension
            );
            var javaScriptFileContent = "// JavaScript Script Comment";

            var csharpFileName = "CSharp.csx";
            var csharpDirectoryFullName = Path.Combine(
                clientScriptsPath,
                "Actions"
            );
            var csharpFileFullName = "csharp-file-full-name";
            var csharpFileExtension = ".csx";
            var csharpFileInfo = new StandardFileInfo(
                csharpFileName,
                csharpDirectoryFullName,
                csharpFileFullName,
                csharpFileExtension
            );
            var csharpFileContent = "// CSharp Script Comment";

            var unknownFileName = "Script.unknown";
            var unknownDirectoryFullName = Path.Combine(
                clientScriptsPath,
                "Actions"
            );
            var unknownFileFullName = "unknown-file-full-name";
            var unknownFileExtension = ".unknown";
            var unknownFileInfo = new StandardFileInfo(
                unknownFileName,
                unknownDirectoryFullName,
                unknownFileFullName,
                unknownFileExtension
            );
            var unknownFileContent = "// Unknown Script Comment";

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var clientScriptRepositoryMock = new Mock<ClientScriptRepository>();

            serverInfoMock.Setup(
                mock => mock.ClientScriptsPath
            ).Returns(
                clientScriptsPath
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
                        javaScriptFileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                javaScriptFileContent
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextFromFile(
                        csharpFileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                csharpFileContent
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextFromFile(
                        unknownFileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                unknownFileContent
            );

            // When
            var handler = new LoadClientScriptsSystemCommandHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                clientScriptRepositoryMock.Object
            );

            await handler.Handle(
                new LoadClientScriptsSystemCommand(),
                CancellationToken.None
            );
            Assert.NotNull(
                onProcessFile
            );

            await onProcessFile(
                javaScriptFileInfo,
                arguments
            );
            await onProcessFile(
                csharpFileInfo,
                arguments
            );
            await onProcessFile(
                unknownFileInfo,
                arguments
            );

            // Then
            clientScriptRepositoryMock.Verify(
                mock => mock.Add(
                    ClientScript.Create(
                        ClientScriptType.JavaScript,
                        "Actions",
                        javaScriptFileName,
                        javaScriptFileContent
                    )
                )
            );
            clientScriptRepositoryMock.Verify(
                mock => mock.Add(
                    ClientScript.Create(
                        ClientScriptType.CSharp,
                        "Actions",
                        csharpFileName,
                        csharpFileContent
                    )
                )
            );
            clientScriptRepositoryMock.Verify(
                mock => mock.Add(
                    ClientScript.Create(
                        ClientScriptType.Unknown,
                        "Actions",
                        unknownFileName,
                        unknownFileContent
                    )
                )
            );
        }
    }
}
