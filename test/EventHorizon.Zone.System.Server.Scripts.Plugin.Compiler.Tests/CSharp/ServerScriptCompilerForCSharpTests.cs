namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Tests.CSharp
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Assemblies;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.CSharp;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Model;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Hasher.Consolidate;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Hasher.Create;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Model;
    using FluentAssertions;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Linq;
    using global::System.Reflection;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class ServerScriptCompilerForCSharpTests
    {
        private static readonly string nl = Environment.NewLine;

        [Fact]
        public async Task ShouldReturnHashAndEncodedFileContentFromAssemblyEvaluator()
        {
            // Given
            var hash = "hash";
            var scriptAssembly = "script-assembly";
            var consolidatedScripts = $"                        {nl}{nl}{nl}script-assembly{nl}";
            var generatedFileFullName = "generated-file-full-name";
            var scriptAssemblyBytes = scriptAssembly.ToBytes();

            var scripts = new List<ServerScriptDetails>();

            var expected = hash;

            var loggerMock = new Mock<ILogger<ServerScriptCompilerForCSharp>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var assemblyBuilderMock = new Mock<AssemblyBuilder>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new ConsolidateServerScriptsCommand(
                        scripts
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateServerScriptsResult>(
                    new ConsolidateServerScriptsResult(
                        new List<string>(),
                        string.Empty,
                        consolidatedScripts
                    )
                )
            );

            assemblyBuilderMock.Setup(
                mock => mock.Compile(
                    consolidatedScripts
                )
            ).ReturnsAsync(
                generatedFileFullName
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextAsBytesFromFile(
                        generatedFileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                scriptAssemblyBytes
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new QueryForScriptAssemblyList(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<Assembly>
                {
                    AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault()
                }
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new CreateHashFromContentCommand(
                        consolidatedScripts
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<string>(
                    true,
                    hash
                )
            );

            // When
            var compiler = new ServerScriptCompilerForCSharp(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                assemblyBuilderMock.Object
            );
            var actual = await compiler.Compile(
                scripts,
                CancellationToken.None
            );

            // Then
            actual.Hash
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnErrorCodeResultOnAnyException()
        {
            // Given
            var fileSystemTempPath = "file-system-temp-path";
            var expected = new CompiledScriptResult(
                false,
                "csharp_failed_to_compile_server_scripts"
            );

            var loggerMock = new Mock<ILogger<ServerScriptCompilerForCSharp>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var assemblyBuilderMock = new Mock<AssemblyBuilder>();

            serverInfoMock.Setup(
                mock => mock.FileSystemTempPath
            ).Returns(
                fileSystemTempPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ConsolidateServerScriptsCommand>(),
                    CancellationToken.None
                )
            ).ThrowsAsync(
                new Exception("error")
            );

            // When
            var compiler = new ServerScriptCompilerForCSharp(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                assemblyBuilderMock.Object
            );
            var actual = await compiler.Compile(
                new List<ServerScriptDetails>(),
                CancellationToken.None
            );


            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public async Task ShouldSaveFileOfConsolidatedScriptsWhenErrorIsCaught()
        {
            // Given
            var scriptClassesConsolidated = "script-classes-consolidated";
            var fileSystemTempPath = "file-system-temp-path";
            var fileFullName = Path.Combine(
                fileSystemTempPath,
                "ConsolidatedServerScripts.csx"
            );
            var consolidatedScripts = $"                        {nl}{nl}{nl}{scriptClassesConsolidated}{nl}";
            var fileContents = consolidatedScripts;

            var scripts = new List<ServerScriptDetails>();

            var expected = new WriteAllTextToFile(
                fileFullName,
                fileContents
            );

            var loggerMock = new Mock<ILogger<ServerScriptCompilerForCSharp>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var assemblyBuilderMock = new Mock<AssemblyBuilder>();

            serverInfoMock.Setup(
                mock => mock.FileSystemTempPath
            ).Returns(
                fileSystemTempPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new ConsolidateServerScriptsCommand(
                        scripts
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateServerScriptsResult>(
                    new ConsolidateServerScriptsResult(
                        new List<string>(),
                        string.Empty,
                        consolidatedScripts
                    )
                )
            );

            assemblyBuilderMock.Setup(
                mock => mock.Compile(
                    It.IsAny<string>()
                )
            ).Throws(
                new Exception("error")
            );

            // When
            var compiler = new ServerScriptCompilerForCSharp(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                assemblyBuilderMock.Object
            );
            var actual = await compiler.Compile(
                scripts,
                CancellationToken.None
            );


            // Then
            actual.Success.Should().BeFalse();

            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldReturnConsolidationErrorCodeWhenConsolidationCommandFails()
        {
            // Given
            var fileSystemTempPath = "file-system-temp-path";
            var errorCode = "FAILED_SERVER_SCRIPT_CONSOLIDATION";

            var expected = errorCode;

            var loggerMock = new Mock<ILogger<ServerScriptCompilerForCSharp>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var assemblyBuilderMock = new Mock<AssemblyBuilder>();

            serverInfoMock.Setup(
                mock => mock.FileSystemTempPath
            ).Returns(
                fileSystemTempPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ConsolidateServerScriptsCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateServerScriptsResult>(
                    errorCode
                )
            );

            // When
            var compiler = new ServerScriptCompilerForCSharp(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                assemblyBuilderMock.Object
            );
            var actual = await compiler.Compile(
                new List<ServerScriptDetails>(),
                CancellationToken.None
            );


            // Then
            actual.ErrorCode.Should().Be(
                expected
            );
        }
    }
}
