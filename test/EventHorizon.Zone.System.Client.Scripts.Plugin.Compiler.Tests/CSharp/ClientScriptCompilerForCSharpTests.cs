namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Tests.CSharp
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.CSharp;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model;
    using FluentAssertions;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class ClientScriptCompilerForCSharpTests
    {
        private static readonly string nl = Environment.NewLine;

        [Fact]
        public async Task ShouldReturnHashAndEncodedFileContentFromAssemblyEvaluator()
        {
            // Given
            var scriptContent = "script-assembly";
            var scriptAssembly = "script-assembly";
            var consolidatedScripts = $"                        {nl}{nl}{nl}script-assembly{nl}";
            var generatedFileFullName = "generated-file-full-name";
            var scriptAssemblyBytes = scriptAssembly.ToBytes();

            var expected = "c2NyaXB0LWFzc2VtYmx5";

            var scripts = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    "ScriptName",
                    "script-path",
                    scriptContent
                )
            };

            var loggerMock = new Mock<ILogger<ClientScriptCompilerForCSharp>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var assemblyBuilderMock = new Mock<AssemblyBuilder>();
            var clientScriptsConsolidatorMock = new Mock<ClientScriptsConsolidator>();

            clientScriptsConsolidatorMock.Setup(
                mock => mock.IntoSingleTemplatedString(
                    scripts,
                    ref It.Ref<List<string>>.IsAny
                )
            ).Returns(
                scriptAssembly
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

            // When
            var compiler = new ClientScriptCompilerForCSharp(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                assemblyBuilderMock.Object,
                clientScriptsConsolidatorMock.Object
            );
            var actual = await compiler.Compile(
                scripts
            );

            // Then
            actual.Hash.Should().NotBeNullOrEmpty();
            actual.ScriptAssembly.Should().Be(
                expected
            );
        }

        [Fact]
        public async Task ShouldReturnErrorCodeResultOnAnyException()
        {
            // Given
            var fileSystemTempPath = "file-system-temp-path";
            var expected = new CompiledScriptResult(
                "csharp_failed_to_compile"
            );

            var loggerMock = new Mock<ILogger<ClientScriptCompilerForCSharp>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var assemblyBuilderMock = new Mock<AssemblyBuilder>();
            var clientScriptsConsolidatorMock = new Mock<ClientScriptsConsolidator>();

            serverInfoMock.Setup(
                mock => mock.FileSystemTempPath
            ).Returns(
                fileSystemTempPath
            );

            clientScriptsConsolidatorMock.Setup(
                mock => mock.IntoSingleTemplatedString(
                    It.IsAny<IEnumerable<ClientScript>>(),
                    ref It.Ref<List<string>>.IsAny
                )
            ).Throws(
                new Exception("error")
            );

            // When
            var compiler = new ClientScriptCompilerForCSharp(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                assemblyBuilderMock.Object,
                clientScriptsConsolidatorMock.Object
            );
            var actual = await compiler.Compile(
                new List<ClientScript>()
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
                "ConsolidatedScripts.csx"
            );
            var consolidatedScripts = $"                        {nl}{nl}{nl}{scriptClassesConsolidated}{nl}";
            var fileContents = consolidatedScripts;
            var expected = new WriteAllTextToFile(
                fileFullName,
                fileContents
            );

            var loggerMock = new Mock<ILogger<ClientScriptCompilerForCSharp>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var assemblyBuilderMock = new Mock<AssemblyBuilder>();
            var clientScriptsConsolidatorMock = new Mock<ClientScriptsConsolidator>();

            serverInfoMock.Setup(
                mock => mock.FileSystemTempPath
            ).Returns(
                fileSystemTempPath
            );

            assemblyBuilderMock.Setup(
                mock => mock.Compile(
                    It.IsAny<string>()
                )
            ).Throws(
                new Exception("error")
            );

            clientScriptsConsolidatorMock.Setup(
                mock => mock.IntoSingleTemplatedString(
                    It.IsAny<IList<ClientScript>>(),
                    ref It.Ref<List<string>>.IsAny
                )
            ).Returns(
                scriptClassesConsolidated
            );

            // When
            var compiler = new ClientScriptCompilerForCSharp(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                assemblyBuilderMock.Object,
                clientScriptsConsolidatorMock.Object
            );
            var actual = await compiler.Compile(
                new List<ClientScript>()
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
    }
}
