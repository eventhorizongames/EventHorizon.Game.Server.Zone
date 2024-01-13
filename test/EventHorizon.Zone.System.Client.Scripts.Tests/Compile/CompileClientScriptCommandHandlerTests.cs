namespace EventHorizon.Zone.System.Client.Scripts.Tests.Compile;

using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.Core.Events.SubProcess;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.Core.Model.SubProcess;
using EventHorizon.Zone.System.Client.Scripts.Api;
using EventHorizon.Zone.System.Client.Scripts.Compile;
using EventHorizon.Zone.System.Client.Scripts.Model;
using EventHorizon.Zone.System.Client.Scripts.Model.Client;
using EventHorizon.Zone.System.Client.Scripts.Model.Generated;
using EventHorizon.Zone.System.Client.Scripts.Validation;

using FluentAssertions;

using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class CompileClientScriptCommandHandlerTests
{
    [Fact]
    public async Task ShouldReturnFailedErrorCodeWhenStartSubProcessFails()
    {
        // Given
        var expectedErrorCode = "failed-starting-of-sub-process";
        var compilerSubProcessDirectory = "compiler-sub-process-directory";
        var compilerSubProcess = "compiler-sub-process";
        var processFullName = Path.Combine(
            compilerSubProcessDirectory,
            compilerSubProcess
        );

        var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();
        var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
        var clientScriptsSettings = new ClientScriptsSettings(
            compilerSubProcessDirectory,
            compilerSubProcess
        );
        var clientScriptsStateMock = new Mock<ClientScriptsState>();

        mediatorMock.Setup(
            mock => mock.Send(
                new StartSubProcessCommand(
                    processFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new CommandResult<SubProcessHandle>(
                expectedErrorCode
            )
        );

        // When
        var handler = new CompileClientScriptCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object,
            jsonFileLoaderMock.Object,
            clientScriptsSettings,
            clientScriptsStateMock.Object
        );
        var actual = await handler.Handle(
            new CompileClientScriptCommand(),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(
            expectedErrorCode
        );
    }

    [Fact]
    public async Task ShouldReturnInvalidProcessErrorCodeWhenProcessExitCodeIsNotZero()
    {
        // Given
        var expectedErrorCode = ClientScriptsErrorCodes.CLIENT_SCRIPT_INVALID_PROCESS_ERROR_CODE;
        var compilerSubProcessDirectory = "compiler-sub-process-directory";
        var compilerSubProcess = "compiler-sub-process";
        var processFullName = Path.Combine(
            compilerSubProcessDirectory,
            compilerSubProcess
        );

        var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();
        var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
        var clientScriptsSettings = new ClientScriptsSettings(
            compilerSubProcessDirectory,
            compilerSubProcess
        );
        var clientScriptsStateMock = new Mock<ClientScriptsState>();

        var subProcessHandleMock = new Mock<SubProcessHandle>();

        subProcessHandleMock.Setup(
            mock => mock.ExitCode
        ).Returns(
            100
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

        // When
        var handler = new CompileClientScriptCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object,
            jsonFileLoaderMock.Object,
            clientScriptsSettings,
            clientScriptsStateMock.Object
        );
        var actual = await handler.Handle(
            new CompileClientScriptCommand(),
            CancellationToken.None
        );


        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(
            expectedErrorCode
        );
    }

    [Fact]
    public async Task ShouldNotRunSubProcessCompiileWhenNeedToCompileValidationCheckReturnsFalse()
    {
        // Given
        var currentHash = "hash";
        var compilerSubProcessDirectory = "compiler-sub-process-directory";
        var compilerSubProcess = "compiler-sub-process";

        var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();
        var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
        var scriptSettings = new ClientScriptsSettings(
            compilerSubProcessDirectory,
            compilerSubProcess
        );
        var stateMock = new Mock<ClientScriptsState>();

        stateMock.Setup(
            mock => mock.Hash
        ).Returns(
            currentHash
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new NeedToCompileClientScripts(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new CommandResult<bool>(
                false
            )
        );

        // When
        var handler = new CompileClientScriptCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object,
            jsonFileLoaderMock.Object,
            scriptSettings,
            stateMock.Object
        );
        var actual = await handler.Handle(
            new CompileClientScriptCommand(),
            CancellationToken.None
        );

        // Then
        actual.Success
            .Should().BeTrue();
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

        var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();
        var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
        var scriptSettings = new ClientScriptsSettings(
            compilerSubProcessDirectory,
            compilerSubProcess
        );
        var stateMock = new Mock<ClientScriptsState>();

        stateMock.Setup(
            mock => mock.Hash
        ).Returns(
            currentHash
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new NeedToCompileClientScripts(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new CommandResult<bool>(
                errorCode
            )
        );

        // When
        var handler = new CompileClientScriptCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object,
            jsonFileLoaderMock.Object,
            scriptSettings,
            stateMock.Object
        );
        var actual = await handler.Handle(
            new CompileClientScriptCommand(),
            CancellationToken.None
        );

        // Then
        actual.Success
            .Should().BeFalse();
        actual.ErrorCode
            .Should().Be(expected);
    }

    [Fact]
    public async Task ShouldReturnFileLoaderErrorCodeWhenNotSuccessful()
    {
        // Given
        var expectedErrorCode = "json-file-loader-error-code";
        var generatedPath = "generate-path";
        var compilerSubProcessDirectory = "compiler-sub-process-directory";
        var compilerSubProcess = "compiler-sub-process";
        var processFullName = Path.Combine(
            compilerSubProcessDirectory,
            compilerSubProcess
        );
        var generatedFilePath = Path.Combine(
            generatedPath,
            GeneratedClientScriptsResultModel.GENERATED_FILE_NAME
        );

        var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();
        var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
        var clientScriptsSettings = new ClientScriptsSettings(
            compilerSubProcessDirectory,
            compilerSubProcess
        );
        var clientScriptsStateMock = new Mock<ClientScriptsState>();

        var subProcessHandleMock = new Mock<SubProcessHandle>();

        serverInfoMock.Setup(
            mock => mock.GeneratedPath
        ).Returns(
            generatedPath
        );

        subProcessHandleMock.Setup(
            mock => mock.ExitCode
        ).Returns(
            0
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

        jsonFileLoaderMock.Setup(
            mock => mock.GetFile<GeneratedClientScriptsResultModel>(
                generatedFilePath
            )
        ).ReturnsAsync(
            new GeneratedClientScriptsResultModel
            {
                Success = false,
                ErrorCode = expectedErrorCode,
            }
        );

        // When
        var handler = new CompileClientScriptCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object,
            jsonFileLoaderMock.Object,
            clientScriptsSettings,
            clientScriptsStateMock.Object
        );
        var actual = await handler.Handle(
            new CompileClientScriptCommand(),
            CancellationToken.None
        );


        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(
            expectedErrorCode
        );
    }

    [Fact]
    public async Task ShouldSetHashAndScriptAssemblyWhenHashIsSetAndNeedToCompileIsTrue()
    {
        // Given
        var hash = "not-current-hash";
        var generatedPath = "generate-path";
        var compilerSubProcessDirectory = "compiler-sub-process-directory";
        var compilerSubProcess = "compiler-sub-process";
        var processFullName = Path.Combine(
            compilerSubProcessDirectory,
            compilerSubProcess
        );
        var generatedFilePath = Path.Combine(
            generatedPath,
            GeneratedClientScriptsResultModel.GENERATED_FILE_NAME
        );

        var expectedHash = "hash";
        var expectedScriptAssembly = "script-assembly";

        var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();
        var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
        var clientScriptsSettings = new ClientScriptsSettings(
            compilerSubProcessDirectory,
            compilerSubProcess
        );
        var clientScriptsStateMock = new Mock<ClientScriptsState>();

        var subProcessHandleMock = new Mock<SubProcessHandle>();

        clientScriptsStateMock.Setup(
            mock => mock.Hash
        ).Returns(
            hash
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new NeedToCompileClientScripts(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new CommandResult<bool>(
                true
            )
        );

        serverInfoMock.Setup(
            mock => mock.GeneratedPath
        ).Returns(
            generatedPath
        );

        subProcessHandleMock.Setup(
            mock => mock.ExitCode
        ).Returns(
            0
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

        jsonFileLoaderMock.Setup(
            mock => mock.GetFile<GeneratedClientScriptsResultModel>(
                generatedFilePath
            )
        ).ReturnsAsync(
            new GeneratedClientScriptsResultModel
            {
                Success = true,
                Hash = expectedHash,
                ScriptAssembly = expectedScriptAssembly,
            }
        );

        // When
        var handler = new CompileClientScriptCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object,
            jsonFileLoaderMock.Object,
            clientScriptsSettings,
            clientScriptsStateMock.Object
        );
        await handler.Handle(
            new CompileClientScriptCommand(),
            CancellationToken.None
        );

        // Then
        clientScriptsStateMock.Verify(
            mock => mock.SetAssembly(
                expectedHash,
                expectedScriptAssembly
            )
        );
    }

    [Fact]
    public async Task ShouldSetHashAndScriptAssemblyWhenComplierSuccessIsTrue()
    {
        // Given
        var generatedPath = "generate-path";
        var compilerSubProcessDirectory = "compiler-sub-process-directory";
        var compilerSubProcess = "compiler-sub-process";
        var processFullName = Path.Combine(
            compilerSubProcessDirectory,
            compilerSubProcess
        );
        var generatedFilePath = Path.Combine(
            generatedPath,
            GeneratedClientScriptsResultModel.GENERATED_FILE_NAME
        );

        var expectedHash = "hash";
        var expectedScriptAssembly = "script-assembly";

        var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();
        var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
        var clientScriptsSettings = new ClientScriptsSettings(
            compilerSubProcessDirectory,
            compilerSubProcess
        );
        var clientScriptsStateMock = new Mock<ClientScriptsState>();

        var subProcessHandleMock = new Mock<SubProcessHandle>();

        serverInfoMock.Setup(
            mock => mock.GeneratedPath
        ).Returns(
            generatedPath
        );

        subProcessHandleMock.Setup(
            mock => mock.ExitCode
        ).Returns(
            0
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

        jsonFileLoaderMock.Setup(
            mock => mock.GetFile<GeneratedClientScriptsResultModel>(
                generatedFilePath
            )
        ).ReturnsAsync(
            new GeneratedClientScriptsResultModel
            {
                Success = true,
                Hash = expectedHash,
                ScriptAssembly = expectedScriptAssembly,
            }
        );

        // When
        var handler = new CompileClientScriptCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object,
            jsonFileLoaderMock.Object,
            clientScriptsSettings,
            clientScriptsStateMock.Object
        );
        await handler.Handle(
            new CompileClientScriptCommand(),
            CancellationToken.None
        );

        // Then
        clientScriptsStateMock.Verify(
            mock => mock.SetAssembly(
                expectedHash,
                expectedScriptAssembly
            )
        );
    }

    [Fact]
    public async Task ShouldPublishClientActionScriptAssemblyChangedEventWhenComplierSuccessIsTrue()
    {
        // Given
        var hash = "hash";
        var scriptAssembly = "script-assembly";
        var generatedPath = "generate-path";
        var compilerSubProcessDirectory = "compiler-sub-process-directory";
        var compilerSubProcess = "compiler-sub-process";
        var processFullName = Path.Combine(
            compilerSubProcessDirectory,
            compilerSubProcess
        );
        var generatedFilePath = Path.Combine(
            generatedPath,
            GeneratedClientScriptsResultModel.GENERATED_FILE_NAME
        );

        var expectedAction = "CLIENT_SCRIPTS_ASSEMBLY_CHANGED_CLIENT_ACTION_EVENT";
        var expectedHash = hash;
        var expectedScriptAssembly = scriptAssembly;

        var loggerMock = new Mock<ILogger<CompileClientScriptCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();
        var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
        var clientScriptsSettings = new ClientScriptsSettings(
            compilerSubProcessDirectory,
            compilerSubProcess
        );
        var clientScriptsStateMock = new Mock<ClientScriptsState>();

        var subProcessHandleMock = new Mock<SubProcessHandle>();

        serverInfoMock.Setup(
            mock => mock.GeneratedPath
        ).Returns(
            generatedPath
        );

        subProcessHandleMock.Setup(
            mock => mock.ExitCode
        ).Returns(
            0
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

        jsonFileLoaderMock.Setup(
            mock => mock.GetFile<GeneratedClientScriptsResultModel>(
                generatedFilePath
            )
        ).ReturnsAsync(
            new GeneratedClientScriptsResultModel
            {
                Success = true,
                Hash = expectedHash,
                ScriptAssembly = expectedScriptAssembly,
            }
        );

        // When
        var handler = new CompileClientScriptCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object,
            jsonFileLoaderMock.Object,
            clientScriptsSettings,
            clientScriptsStateMock.Object
        );
        var actual = default(ClientActionGenericToAllEvent);

        mediatorMock.Setup(
            mock => mock.Publish(
                It.IsAny<ClientActionGenericToAllEvent>(),
                CancellationToken.None
            )
        ).Callback<ClientActionGenericToAllEvent, CancellationToken>(
            (notification, _) =>
            {
                actual = notification;
            }
        );
        await handler.Handle(
            new CompileClientScriptCommand(),
            CancellationToken.None
        );

        // Then
        actual.Action.Should().Be(
            expectedAction
        );
        actual.Data.Should().BeOfType(
            typeof(ClientScriptsAssemblyChangedClientActionData)
        );

        var actionData = (ClientScriptsAssemblyChangedClientActionData)actual.Data;
        actionData.Hash.Should().Be(
            expectedHash
        );
    }
}
