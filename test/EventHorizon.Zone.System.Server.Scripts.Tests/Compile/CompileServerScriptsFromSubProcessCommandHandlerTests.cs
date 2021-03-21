namespace EventHorizon.Zone.System.Server.Scripts.Tests.Compile
{
    using EventHorizon.Zone.Core.Events.SubProcess;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.Core.Model.SubProcess;
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Complie;
    using EventHorizon.Zone.System.Server.Scripts.Load;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.Model.Generated;
    using EventHorizon.Zone.System.Server.Scripts.Validation;
    using FluentAssertions;
    using global::System;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class CompileServerScriptsFromSubProcessCommandHandlerTests
    {
        [Fact]
        public async Task ShouldUpdateHashOnStateWhenSuccessfullyCompiledSource()
        {
            // Given
            var hash = "hash";
            var notNullOrEmptyHash = "not-null-or-empty";
            var generatedPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "App_Data",
                "Generated"
            );
            var compilerSubProcessDirectory = "compiler-sub-process-directory";
            var compilerSubProcess = "compiler-sub-process";
            var processFullName = Path.Combine(
                compilerSubProcessDirectory,
                compilerSubProcess
            );
            var subProcessErrorCode = 0;
            var generatedScriptResultFullName = Path.Combine(
                generatedPath,
                GeneratedServerScriptsResultModel.SCRIPTS_RESULT_FILE_NAME
            );
            var compiledResult = new GeneratedServerScriptsResultModel
            {
                Success = true,
                Hash = hash,
            };

            var expected = hash;

            var loggerMock = new Mock<ILogger<CompileServerScriptsFromSubProcessCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            var scriptSettings = new ServerScriptsSettings(
                compilerSubProcessDirectory,
                compilerSubProcess
            );
            var stateMock = new Mock<ServerScriptsState>();

            var subProcessHandleMock = new Mock<SubProcessHandle>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new NeedToCompileServerScripts(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<bool>(true)
            );

            serverInfoMock.Setup(
                mock => mock.GeneratedPath
            ).Returns(
                generatedPath
            );

            stateMock.Setup(
                mock => mock.CurrentHash
            ).Returns(
                notNullOrEmptyHash
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new StartSubProcessCommand(
                        processFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SubProcessHandle>(
                    subProcessHandleMock.Object
                )
            );
            subProcessHandleMock.Setup(
                mock => mock.ExitCode
            ).Returns(
                subProcessErrorCode
            );

            jsonFileLoaderMock.Setup(
                mock => mock.GetFile<GeneratedServerScriptsResultModel>(
                    generatedScriptResultFullName
                )
            ).ReturnsAsync(
                compiledResult
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new LoadNewServerScriptAssemblyCommand(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new StandardCommandResult()
            );

            // When
            var handler = new CompileServerScriptsFromSubProcessCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                jsonFileLoaderMock.Object,
                scriptSettings,
                stateMock.Object
            );
            var actual = await handler.Handle(
                new CompileServerScriptsFromSubProcessCommand(

                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();

            stateMock.Verify(
                mock => mock.UpdateHash(
                    expected
                )
            );
        }

        [Fact]
        public async Task ShouldReturnErrorCodeWhenNeedToCompileValidationCheckFails()
        {
            // Given
            var currentHash = "hash";
            var compilerSubProcessDirectory = "compiler-sub-process-directory";
            var compilerSubProcess = "compiler-sub-process";

            var errorCode = "error-code";
            var expected = errorCode;

            var loggerMock = new Mock<ILogger<CompileServerScriptsFromSubProcessCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            var scriptSettings = new ServerScriptsSettings(
                compilerSubProcessDirectory,
                compilerSubProcess
            );
            var stateMock = new Mock<ServerScriptsState>();

            stateMock.Setup(
                mock => mock.CurrentHash
            ).Returns(
                currentHash
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new NeedToCompileServerScripts(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<bool>(
                    errorCode
                )
            );

            // When
            var handler = new CompileServerScriptsFromSubProcessCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                jsonFileLoaderMock.Object,
                scriptSettings,
                stateMock.Object
            );
            var actual = await handler.Handle(
                new CompileServerScriptsFromSubProcessCommand(

                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldNotRunSubProcessCompiileWhenNeedToCompileValidationCheckReturnsFalse()
        {
            // Given
            var currentHash = "hash";
            var compilerSubProcessDirectory = "compiler-sub-process-directory";
            var compilerSubProcess = "compiler-sub-process";

            var loggerMock = new Mock<ILogger<CompileServerScriptsFromSubProcessCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            var scriptSettings = new ServerScriptsSettings(
                compilerSubProcessDirectory,
                compilerSubProcess
            );
            var stateMock = new Mock<ServerScriptsState>();

            stateMock.Setup(
                mock => mock.CurrentHash
            ).Returns(
                currentHash
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new NeedToCompileServerScripts(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<bool>(
                    false
                )
            );

            // When
            var handler = new CompileServerScriptsFromSubProcessCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                jsonFileLoaderMock.Object,
                scriptSettings,
                stateMock.Object
            );
            var actual = await handler.Handle(
                new CompileServerScriptsFromSubProcessCommand(

                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnErrorCodeWhenSubProcessStartFails()
        {
            // Given
            var notNullOrEmptyHash = "not-null-or-empty";
            var generatedPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "App_Data",
                "Generated"
            );
            var compilerSubProcessDirectory = "compiler-sub-process-directory";
            var compilerSubProcess = "compiler-sub-process";
            var processFullName = Path.Combine(
                compilerSubProcessDirectory,
                compilerSubProcess
            );
            var errorCode = "error-code";
            var expected = errorCode;

            var loggerMock = new Mock<ILogger<CompileServerScriptsFromSubProcessCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            var scriptSettings = new ServerScriptsSettings(
                compilerSubProcessDirectory,
                compilerSubProcess
            );
            var stateMock = new Mock<ServerScriptsState>();

            var subProcessHandleMock = new Mock<SubProcessHandle>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new NeedToCompileServerScripts(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<bool>(true)
            );

            serverInfoMock.Setup(
                mock => mock.GeneratedPath
            ).Returns(
                generatedPath
            );

            stateMock.Setup(
                mock => mock.CurrentHash
            ).Returns(
                notNullOrEmptyHash
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new StartSubProcessCommand(
                        processFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SubProcessHandle>(
                    errorCode
                )
            );

            // When
            var handler = new CompileServerScriptsFromSubProcessCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                jsonFileLoaderMock.Object,
                scriptSettings,
                stateMock.Object
            );
            var actual = await handler.Handle(
                new CompileServerScriptsFromSubProcessCommand(

                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnErrorCodeWhenSubProcessExitCodeIsNotZero()
        {
            // Given
            var notNullOrEmptyHash = "not-null-or-empty";
            var generatedPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "App_Data",
                "Generated"
            );
            var compilerSubProcessDirectory = "compiler-sub-process-directory";
            var compilerSubProcess = "compiler-sub-process";
            var processFullName = Path.Combine(
                compilerSubProcessDirectory,
                compilerSubProcess
            );
            var subProcessErrorCode = 123;

            var expected = ServerScriptsErrorCodes.SERVER_SCRIPT_INVALID_PROCESS_ERROR_CODE;

            var loggerMock = new Mock<ILogger<CompileServerScriptsFromSubProcessCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            var scriptSettings = new ServerScriptsSettings(
                compilerSubProcessDirectory,
                compilerSubProcess
            );
            var stateMock = new Mock<ServerScriptsState>();

            var subProcessHandleMock = new Mock<SubProcessHandle>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new NeedToCompileServerScripts(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<bool>(true)
            );

            serverInfoMock.Setup(
                mock => mock.GeneratedPath
            ).Returns(
                generatedPath
            );

            stateMock.Setup(
                mock => mock.CurrentHash
            ).Returns(
                notNullOrEmptyHash
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new StartSubProcessCommand(
                        processFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SubProcessHandle>(
                    subProcessHandleMock.Object
                )
            );
            subProcessHandleMock.Setup(
                mock => mock.ExitCode
            ).Returns(
                subProcessErrorCode
            );

            // When
            var handler = new CompileServerScriptsFromSubProcessCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                jsonFileLoaderMock.Object,
                scriptSettings,
                stateMock.Object
            );
            var actual = await handler.Handle(
                new CompileServerScriptsFromSubProcessCommand(

                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnErrorCodeWhenGeneratedServerScriptResultIsNotSuccessful()
        {
            // Given
            var errorCode = "error-code";

            var notNullOrEmptyHash = "not-null-or-empty";
            var generatedPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "App_Data",
                "Generated"
            );
            var compilerSubProcessDirectory = "compiler-sub-process-directory";
            var compilerSubProcess = "compiler-sub-process";
            var processFullName = Path.Combine(
                compilerSubProcessDirectory,
                compilerSubProcess
            );
            var subProcessErrorCode = 0;
            var generatedScriptResultFullName = Path.Combine(
                generatedPath,
                GeneratedServerScriptsResultModel.SCRIPTS_RESULT_FILE_NAME
            );
            var compiledResult = new GeneratedServerScriptsResultModel
            {
                Success = false,
                ErrorCode = errorCode,
            };

            var expected = errorCode;

            var loggerMock = new Mock<ILogger<CompileServerScriptsFromSubProcessCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            var scriptSettings = new ServerScriptsSettings(
                compilerSubProcessDirectory,
                compilerSubProcess
            );
            var stateMock = new Mock<ServerScriptsState>();

            var subProcessHandleMock = new Mock<SubProcessHandle>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new NeedToCompileServerScripts(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<bool>(true)
            );

            serverInfoMock.Setup(
                mock => mock.GeneratedPath
            ).Returns(
                generatedPath
            );

            stateMock.Setup(
                mock => mock.CurrentHash
            ).Returns(
                notNullOrEmptyHash
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new StartSubProcessCommand(
                        processFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SubProcessHandle>(
                    subProcessHandleMock.Object
                )
            );
            subProcessHandleMock.Setup(
                mock => mock.ExitCode
            ).Returns(
                subProcessErrorCode
            );

            jsonFileLoaderMock.Setup(
                mock => mock.GetFile<GeneratedServerScriptsResultModel>(
                    generatedScriptResultFullName
                )
            ).ReturnsAsync(
                compiledResult
            );


            // When
            var handler = new CompileServerScriptsFromSubProcessCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                jsonFileLoaderMock.Object,
                scriptSettings,
                stateMock.Object
            );
            var actual = await handler.Handle(
                new CompileServerScriptsFromSubProcessCommand(

                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }


        [Fact]
        public async Task ShouldReturnErrorCodeWhenExceptionIsThrown()
        {
            // Given
            var currentHash = "hash";
            var compilerSubProcessDirectory = "compiler-sub-process-directory";
            var compilerSubProcess = "compiler-sub-process";

            var expected = ServerScriptsErrorCodes.SERVER_SCRIPTS_FAILED_TO_COMPILE;

            var loggerMock = new Mock<ILogger<CompileServerScriptsFromSubProcessCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            var scriptSettings = new ServerScriptsSettings(
                compilerSubProcessDirectory,
                compilerSubProcess
            );
            var stateMock = new Mock<ServerScriptsState>();

            stateMock.Setup(
                mock => mock.CurrentHash
            ).Returns(
                currentHash
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new NeedToCompileServerScripts(),
                    CancellationToken.None
                )
            ).ThrowsAsync(
                new Exception(
                    "error"
                )
            );

            // When
            var handler = new CompileServerScriptsFromSubProcessCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                jsonFileLoaderMock.Object,
                scriptSettings,
                stateMock.Object
            );
            var actual = await handler.Handle(
                new CompileServerScriptsFromSubProcessCommand(

                ),
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
