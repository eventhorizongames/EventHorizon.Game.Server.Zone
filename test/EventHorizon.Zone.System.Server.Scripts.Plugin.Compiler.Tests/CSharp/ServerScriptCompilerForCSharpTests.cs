namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Tests.CSharp;

using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Server.Scripts.Model.Details;
using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Api;
using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Assemblies;
using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.CSharp;
using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Model;
using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Consolidate;
using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Create;
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
    private static readonly string NL = Environment.NewLine;

    [Fact]
    public async Task ShouldReturnHashAndEncodedFileContentFromAssemblyEvaluator()
    {
        // Given
        var hash = "hash";
        var scriptAssembly = "script-assembly";
        var consolidatedScripts = $"                        {NL}{NL}{NL}script-assembly{NL}";
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
            "csharp_failed_to_compile_server_scripts",
            new List<GeneratedServerScriptErrorDetailsModel>()
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
        actual.Success.Should().BeFalse();
        actual.Hash.Should().BeEmpty();
        actual.ErrorCode.Should().BeSameAs(
            expected.ErrorCode
        );
        actual.ScriptErrorDetailsList.Should().BeEmpty();
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
        var consolidatedScripts = $"                        {NL}{NL}{NL}{scriptClassesConsolidated}{NL}";
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

    [Fact]
    public async Task ShouldReturnErrorDetailsWhenValidErrorCodeIsReturned()
    {
        // Given
        var fileSystemTempPath = "file-system-temp-path";
        var consolidatedScripts = string.Join(
            Environment.NewLine,
            new List<string>
            {
                "// === FILE_START ===",
                "// Script Id: First_File.csx",
                "First file content",
                "First file with error",
                "// === FILE_END ===",
                "",

                "// === FILE_START ===",
                "// Script Id: Second_File.csx",
                "Second file with content",
                "Second file with error",
                "// === FILE_END ===",
                "",

                "// === FILE_START ===",
                "// Script Id: Third_File.csx",
                "Third file content",
                "Third file with error",
                "Third file another",
                "// === FILE_END ===",
            }
        );
        var errorMessage = string.Join(
            Environment.NewLine,
            "(4,47): error CS0103: The name 'listOfPlayer' does not exist in the current context",
            "(15, 56): error CS0103: The name 'arg' does not exist in the current context",
            string.Empty
        );

        var expected = new List<GeneratedServerScriptErrorDetailsModel>
        {
            new GeneratedServerScriptErrorDetailsModel
            {
                Column = 47,
                ErrorLineContent = "First file with error",
                Message = "CS0103: The name 'listOfPlayer' does not exist in the current context",
                ScriptId = "First_File.csx",
            },
            new GeneratedServerScriptErrorDetailsModel
            {
                Column = 56,
                ErrorLineContent = "Third file content",
                Message = "CS0103: The name 'arg' does not exist in the current context",
                ScriptId = "Third_File.csx",
            },
        };

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
                new ConsolidateServerScriptsResult(
                    null,
                    null,
                    consolidatedScripts
                )
            )
        );

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<QueryForScriptAssemblyList>(),
                CancellationToken.None
            )
        ).ThrowsAsync(
            new Exception(errorMessage)
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
        actual.ScriptErrorDetailsList.Should().BeEquivalentTo(
            expected
        );
    }
}
