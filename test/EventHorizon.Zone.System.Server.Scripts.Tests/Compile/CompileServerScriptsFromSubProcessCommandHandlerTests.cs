namespace EventHorizon.Zone.System.Server.Scripts.Tests.Compile
{
    using EventHorizon.Zone.Core.Events.SubProcess;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.Core.Model.SubProcess;
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Complie;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Model.Generated;
    using EventHorizon.Zone.System.Server.Scripts.State;
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
            // This is an complied script available in the generated Server_Scripts.dll
            var scriptId1 = "Admin_Map_ReloadCoreMap";
            var scriptDetails1 = new ServerScriptDetails(
                "script-1-file-name",
                "script-1-file-path",
                "script-1-string"
            );
            // This is an complied script available in the generated Server_Scripts.dll
            var scriptId2 = "Admin_I18n_ReloadI18nSystem";
            var scriptDetails2 = new ServerScriptDetails(
                "script-2-file-name",
                "script-2-file-path",
                "script-2-string"
            );

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
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();
            var scriptRepositoryMock = new Mock<ServerScriptRepository>();

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

            detailsRepositoryMock.Setup(
                mock => mock.Find(
                    scriptId1
                )
            ).Returns(
                scriptDetails1
            );
            detailsRepositoryMock.Setup(
                mock => mock.Find(
                    scriptId2
                )
            ).Returns(
                scriptDetails2
            );


            // When
            var handler = new CompileServerScriptsFromSubProcessCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                jsonFileLoaderMock.Object,
                scriptSettings,
                stateMock.Object,
                detailsRepositoryMock.Object,
                scriptRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new CompileServerScriptsFromSubProcessCommand(

                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();

            detailsRepositoryMock.Verify(
                mock => mock.Add(
                    scriptId1,
                    It.Is<ServerScriptDetails>(
                        a => a.Id == scriptId1
                            && a.Hash == scriptDetails1.Hash
                            && a.FileName == scriptDetails1.FileName
                            && a.Path == scriptDetails1.Path
                            && a.ScriptString == scriptDetails1.ScriptString
                    )
                )
            );
            detailsRepositoryMock.Verify(
                mock => mock.Add(
                    scriptId2,
                    It.Is<ServerScriptDetails>(
                        a => a.Id == scriptId2
                            && a.Hash == scriptDetails2.Hash
                            && a.FileName == scriptDetails2.FileName
                            && a.Path == scriptDetails2.Path
                            && a.ScriptString == scriptDetails2.ScriptString
                    )
                )
            );

            scriptRepositoryMock.Verify(
                mock => mock.Add(
                    It.Is<ServerScript>(
                        a => a.Id == scriptId1
                    )
                )
            );
            scriptRepositoryMock.Verify(
                mock => mock.Add(
                    It.Is<ServerScript>(
                        a => a.Id == scriptId2
                    )
                )
            );

            stateMock.Verify(
                mock => mock.UpdateHash(
                    expected
                )
            );

            scriptRepositoryMock.Verify(
                mock => mock.Clear()
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
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();
            var scriptRepositoryMock = new Mock<ServerScriptRepository>();

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
                stateMock.Object,
                detailsRepositoryMock.Object,
                scriptRepositoryMock.Object
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
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();
            var scriptRepositoryMock = new Mock<ServerScriptRepository>();

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
                stateMock.Object,
                detailsRepositoryMock.Object,
                scriptRepositoryMock.Object
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
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();
            var scriptRepositoryMock = new Mock<ServerScriptRepository>();

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
                stateMock.Object,
                detailsRepositoryMock.Object,
                scriptRepositoryMock.Object
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
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();
            var scriptRepositoryMock = new Mock<ServerScriptRepository>();

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
                stateMock.Object,
                detailsRepositoryMock.Object,
                scriptRepositoryMock.Object
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
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();
            var scriptRepositoryMock = new Mock<ServerScriptRepository>();

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
                stateMock.Object,
                detailsRepositoryMock.Object,
                scriptRepositoryMock.Object
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
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();
            var scriptRepositoryMock = new Mock<ServerScriptRepository>();

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
                stateMock.Object,
                detailsRepositoryMock.Object,
                scriptRepositoryMock.Object
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
