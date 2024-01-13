namespace EventHorizon.Zone.System.Server.Scripts.Tests.Load;

using AutoFixture.Xunit2;

using EventHorizon.Observer.Model;
using EventHorizon.Observer.State;
using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Server.Scripts.Api;
using EventHorizon.Zone.System.Server.Scripts.Load;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.Model.Details;
using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Model;
using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Register;
using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Remove;
using EventHorizon.Zone.System.Server.Scripts.State;

using FluentAssertions;

using global::System;
using global::System.Collections.Generic;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class LoadNewServerScriptAssemblyCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldAddScriptToDetailsAndRepositoryWhenTypesAreFound(
        // Given
        ServerScriptDetails scriptDetails1,
        ServerScriptDetails scriptDetails2,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        [Frozen] Mock<ServerScriptDetailsRepository> detailsRepositoryMock,
        [Frozen] Mock<ServerScriptRepository> scriptRepositoryMock,
        LoadNewServerScriptAssemblyCommandHandler handler
    )
    {
        var generatedPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "App_Data",
            "Generated"
        );
        // This is an complied script available in the generated Server_Scripts.dll
        var scriptId1 = "Admin_Map_ReloadCoreMap";
        // This is an complied script available in the generated Server_Scripts.dll
        var scriptId2 = "Admin_I18n_ReloadI18nSystem";

        serverInfoMock.Setup(
            mock => mock.GeneratedPath
        ).Returns(
            generatedPath
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
        var actual = await handler.Handle(
            new LoadNewServerScriptAssemblyCommand(

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

        scriptRepositoryMock.Verify(
            mock => mock.Clear()
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldRegisterScriptWhenIsObserverBaseType(
        // Given
        ServerScriptDetails scriptDetails1,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        [Frozen] Mock<ServerScriptDetailsRepository> detailsRepositoryMock,
        [Frozen] Mock<ObserverState> observerStateMock,
        LoadNewServerScriptAssemblyCommandHandler handler
    )
    {
        var generatedPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "App_Data",
            "Generated"
        );
        // Interaction_TestInteractionObserver
        // This is an complied script available in the generated Server_Scripts.dll
        var scriptId1 = "Interaction_TestInteractionObserver";

        serverInfoMock.Setup(
            mock => mock.GeneratedPath
        ).Returns(
            generatedPath
        );

        detailsRepositoryMock.Setup(
            mock => mock.Find(
                scriptId1
            )
        ).Returns(
            scriptDetails1
        );

        // When
        var actual = await handler.Handle(
            new LoadNewServerScriptAssemblyCommand(

            ),
            CancellationToken.None
        );

        // Then
        actual.Success
            .Should().BeTrue();

        observerStateMock.Verify(
            mock => mock.Register(
                It.IsAny<ObserverBase>()
            )
        );

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
    }

    [Theory, AutoMoqData]
    public async Task ShouldRemoveScriptFromObserverStateWhenIsObserverBasedScript(
        // Given
        ObserverBasedScript expected,
        ServerScriptDetails scriptDetails1,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        [Frozen] Mock<ServerScriptDetailsRepository> detailsRepositoryMock,
        [Frozen] Mock<ObserverState> observerStateMock,
        [Frozen] Mock<ServerScriptRepository> scriptRepositoryMock,
        LoadNewServerScriptAssemblyCommandHandler handler
    )
    {
        // Given
        var generatedPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "App_Data",
            "Generated"
        );
        // Interaction_TestInteractionObserver
        // This is an complied script available in the generated Server_Scripts.dll
        var scriptId1 = "Interaction_TestInteractionObserver";

        serverInfoMock.Setup(
            mock => mock.GeneratedPath
        ).Returns(
            generatedPath
        );

        detailsRepositoryMock.Setup(
            mock => mock.Find(
                scriptId1
            )
        ).Returns(
            scriptDetails1
        );

        scriptRepositoryMock.Setup(
            mock => mock.All
        ).Returns(
            new List<ServerScript>
            {
                expected,
            }
        );

        // When
        var actual = await handler.Handle(
            new LoadNewServerScriptAssemblyCommand(),
            CancellationToken.None
        );

        // Then
        observerStateMock.Verify(
            mock => mock.Remove(
                expected
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldCallDisposeOnScriptWhenImplementsIDisposable(
        // Given
        ServerScriptDetails scriptDetails1,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        [Frozen] Mock<ServerScriptDetailsRepository> detailsRepositoryMock,
        [Frozen] Mock<ServerScriptRepository> scriptRepositoryMock,
        LoadNewServerScriptAssemblyCommandHandler handler
    )
    {
        DisposableBasedScript disposableBasedScript = new();
        var generatedPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "App_Data",
            "Generated"
        );
        // Interaction_TestInteractionObserver
        // This is an complied script available in the generated Server_Scripts.dll
        var scriptId1 = "Interaction_TestInteractionObserver";

        serverInfoMock.Setup(
            mock => mock.GeneratedPath
        ).Returns(
            generatedPath
        );

        detailsRepositoryMock.Setup(
            mock => mock.Find(
                scriptId1
            )
        ).Returns(
            scriptDetails1
        );

        scriptRepositoryMock.Setup(
            mock => mock.All
        ).Returns(
            new List<ServerScript>
            {
                disposableBasedScript
            }
        );

        // When
        var actual = await handler.Handle(
            new LoadNewServerScriptAssemblyCommand(),
            CancellationToken.None
        );

        // Then
        disposableBasedScript.DisposedCalled
            .Should().Be(1);
    }

    [Theory, AutoMoqData]
    public async Task ShouldReturnErrorCodeWhenAnyExceptionIsThrown(
        // Given
        LoadNewServerScriptAssemblyCommandHandler handler
    )
    {
        var expected = "SERVER_SCRIPT_FAILED_LOADING_ASSEMBLY_ERROR_CODE";

        // When
        var actual = await handler.Handle(
            new LoadNewServerScriptAssemblyCommand(),
            CancellationToken.None
        );

        // Then
        actual.Success
            .Should().BeFalse();

        actual.ErrorCode
            .Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ShouldRegisterBackgroundTaskWhenIsBackgroundTaskBaseType(
        // Given
        ServerScriptDetails scriptDetails1,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<ServerScriptDetailsRepository> detailsRepositoryMock,
        LoadNewServerScriptAssemblyCommandHandler handler
    )
    {
        var generatedPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "App_Data",
            "Generated"
        );
        // Tasks_TestBackgroundTask
        // This is an complied script available in the generated Server_Scripts.dll
        var scriptId1 = "Tasks_TestBackgroundTask";
        var expected = "Tasks_TestBackgroundTask";

        serverInfoMock.Setup(
            mock => mock.GeneratedPath
        ).Returns(
            generatedPath
        );

        detailsRepositoryMock.Setup(
            mock => mock.Find(
                scriptId1
            )
        ).Returns(
            scriptDetails1
        );

        // When
        var actual = await handler.Handle(
            new LoadNewServerScriptAssemblyCommand(

            ),
            CancellationToken.None
        );

        // Then
        actual.Success
            .Should().BeTrue();

        mediatorMock.Verify(
            mock => mock.Send(
                It.Is<RegisterNewScriptedBackgroundTaskCommand>(
                    a => a.BackgroundTask.Id == expected
                ),
                CancellationToken.None
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldRemoveBackgroundTaskWhenRegisteredBackgroundTaskIsInRepository(
        // Given
        TestScriptedBackgroundTask expected,
        ServerScriptDetails scriptDetails,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        [Frozen] Mock<ServerScriptDetailsRepository> detailsRepositoryMock,
        [Frozen] Mock<ServerScriptRepository> scriptRepositoryMock,
        LoadNewServerScriptAssemblyCommandHandler handler
    )
    {
        // Given
        var generatedPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "App_Data",
            "Generated"
        );
        // Tasks_TestBackgroundTask
        // This is an complied script available in the generated Server_Scripts.dll
        var scriptId = "Tasks_TestBackgroundTask";

        serverInfoMock.Setup(
            mock => mock.GeneratedPath
        ).Returns(
            generatedPath
        );

        detailsRepositoryMock.Setup(
            mock => mock.Find(
                scriptId
            )
        ).Returns(
            scriptDetails
        );

        scriptRepositoryMock.Setup(
            mock => mock.All
        ).Returns(
            new List<ServerScript>
            {
                expected,
            }
        );

        // When
        var actual = await handler.Handle(
            new LoadNewServerScriptAssemblyCommand(),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                new RemoveScriptedBackgroundTaskCommand(
                    expected.Id
                ),
                CancellationToken.None
            )
        );
    }

    public class ObserverBasedScriptArgs
    {

    }

    public class ObserverBasedScript
        : ServerScript,
        ArgumentObserver<ObserverBasedScriptArgs>
    {
        public string Id { get; }
        public IEnumerable<string> Tags { get; }

        public Task Handle(
            ObserverBasedScriptArgs args
        )
        {
            return default;
        }

        public Task<ServerScriptResponse> Run(
            ServerScriptServices services,
            ServerScriptData data
        )
        {
            return default;
        }
    }

    public class TestScriptedBackgroundTask
        : ScriptedBackgroundTask
    {
        public string Id { get; }
        public IEnumerable<string> Tags { get; }
        public string TaskId { get; }
        public int TaskPeriod { get; }
        public IEnumerable<string> TaskTags { get; }

        public Task<ServerScriptResponse> Run(
            ServerScriptServices services,
            ServerScriptData data
        )
        {
            return default;
        }

        public Task TaskTrigger(
            ServerScriptServices services
        )
        {
            return Task.CompletedTask;
        }
    }

    public class DisposableBasedScript
        : ServerScript,
        IDisposable
    {
        public string Id { get; }
        public IEnumerable<string> Tags { get; }

        public int DisposedCalled;

        public void Dispose()
        {
            DisposedCalled++;
        }

        public Task<ServerScriptResponse> Run(
            ServerScriptServices services,
            ServerScriptData data
        )
        {
            return default;
        }
    }
}
