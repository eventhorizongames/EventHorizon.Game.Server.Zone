namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Tests.CSharp
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Assemblies;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.CSharp;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Consolidate;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Create;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Model;

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

    public class ClientScriptCompilerForCSharpTests
    {
        private static readonly string nl = Environment.NewLine;

        [Fact]
        public async Task ShouldReturnHashAndEncodedFileContentFromAssemblyEvaluator()
        {
            // Given
            var usingList = new List<string>();
            var scriptClasses = "script-classes";
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

            mediatorMock.Setup(
                mock => mock.Send(
                    new ConsolidateClientScriptsCommand(
                        scripts
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateClientScriptsResult>(
                    new ConsolidateClientScriptsResult(
                        usingList,
                        scriptClasses,
                        consolidatedScripts
                    )
                )
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
                    expected
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

            // When
            var compiler = new ClientScriptCompilerForCSharp(
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
                false,
                "csharp_failed_to_compile"
            );

            var loggerMock = new Mock<ILogger<ClientScriptCompilerForCSharp>>();
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
                    It.IsAny<ConsolidateClientScriptsCommand>(),
                    CancellationToken.None
                )
            ).Throws(
                new Exception()
            );

            // When
            var compiler = new ClientScriptCompilerForCSharp(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                assemblyBuilderMock.Object
            );
            var actual = await compiler.Compile(
                new List<ClientScript>(),
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
            var scripts = new List<ClientScript>();
            var usingList = new List<string>();
            var scriptClasses = "script-classes";
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

            serverInfoMock.Setup(
                mock => mock.FileSystemTempPath
            ).Returns(
                fileSystemTempPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new ConsolidateClientScriptsCommand(
                        scripts
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateClientScriptsResult>(
                    new ConsolidateClientScriptsResult(
                        usingList,
                        scriptClasses,
                        consolidatedScripts
                    )
                )
            );

            assemblyBuilderMock.Setup(
                mock => mock.Compile(
                    consolidatedScripts
                )
            ).Throws(
                new Exception("error")
            );

            // When
            var compiler = new ClientScriptCompilerForCSharp(
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
        public async Task ShouldReturnErrorWhenConsolidatedClientScriptsReturnsFailure()
        {
            // Given
            var errorCode = "error-code";
            var expected = "error-code";

            var scripts = new List<ClientScript>();

            var loggerMock = new Mock<ILogger<ClientScriptCompilerForCSharp>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var assemblyBuilderMock = new Mock<AssemblyBuilder>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new ConsolidateClientScriptsCommand(
                        scripts
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<ConsolidateClientScriptsResult>(
                    errorCode
                )
            );

            // When
            var compiler = new ClientScriptCompilerForCSharp(
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
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }
    }
}
