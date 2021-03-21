namespace EventHorizon.Zone.System.Server.Scripts.Tests.Load
{
    using EventHorizon.Observer.Model;
    using EventHorizon.Observer.State;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Load;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using FluentAssertions;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class LoadNewServerScriptAssemblyCommandHandlerTests
    {
        [Fact]
        public async Task ShouldAddScriptToDetailsAndRepositoryWhenTypesAreFound()
        {
            // Given
            var generatedPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "App_Data",
                "Generated"
            );
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

            var loggerMock = new Mock<ILogger<LoadNewServerScriptAssemblyCommandHandler>>();
            var serverInfoMock = new Mock<ServerInfo>();
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();
            var scriptRepositoryMock = new Mock<ServerScriptRepository>();
            var observerStateMock = new Mock<ObserverState>();
            var serverScriptServicesMock = new Mock<ServerScriptServices>();

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

            serverScriptServicesMock.Setup(
                mock => mock.Logger<It.IsAnyType>()
            ).Returns(
                loggerMock.Object
            );

            // When
            var handler = new LoadNewServerScriptAssemblyCommandHandler(
                loggerMock.Object,
                serverInfoMock.Object,
                detailsRepositoryMock.Object,
                scriptRepositoryMock.Object,
                observerStateMock.Object,
                serverScriptServicesMock.Object
            );
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

        [Fact]
        public async Task ShouldRegisterScriptWhenIsObserverBaseType()
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
            var scriptDetails1 = new ServerScriptDetails(
                "script-1-file-name",
                "script-1-file-path",
                "script-1-string"
            );

            var loggerMock = new Mock<ILogger<LoadNewServerScriptAssemblyCommandHandler>>();
            var serverInfoMock = new Mock<ServerInfo>();
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();
            var scriptRepositoryMock = new Mock<ServerScriptRepository>();
            var observerStateMock = new Mock<ObserverState>();
            var serverScriptServicesMock = new Mock<ServerScriptServices>();

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

            serverScriptServicesMock.Setup(
                mock => mock.Logger<It.IsAnyType>()
            ).Returns(
                loggerMock.Object
            );

            // When
            var handler = new LoadNewServerScriptAssemblyCommandHandler(
                loggerMock.Object,
                serverInfoMock.Object,
                detailsRepositoryMock.Object,
                scriptRepositoryMock.Object,
                observerStateMock.Object,
                serverScriptServicesMock.Object
            );
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

        [Fact]
        public async Task ShouldRemoveScriptFromObserverStateWhenIsObserverBasedScript()
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
            var scriptDetails1 = new ServerScriptDetails(
                "script-1-file-name",
                "script-1-file-path",
                "script-1-string"
            );

            var observerBasedScript = new ObserverBasedScript();
            var expected = observerBasedScript;

            var loggerMock = new Mock<ILogger<LoadNewServerScriptAssemblyCommandHandler>>();
            var serverInfoMock = new Mock<ServerInfo>();
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();
            var scriptRepositoryMock = new Mock<ServerScriptRepository>();
            var observerStateMock = new Mock<ObserverState>();
            var serverScriptServicesMock = new Mock<ServerScriptServices>();

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
                    observerBasedScript
                }
            );

            serverScriptServicesMock.Setup(
                mock => mock.Logger<It.IsAnyType>()
            ).Returns(
                loggerMock.Object
            );

            // When
            var handler = new LoadNewServerScriptAssemblyCommandHandler(
                loggerMock.Object,
                serverInfoMock.Object,
                detailsRepositoryMock.Object,
                scriptRepositoryMock.Object,
                observerStateMock.Object,
                serverScriptServicesMock.Object
            );
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

        [Fact]
        public async Task ShouldCallDisposeOnScriptWhenImplementsIDisposable()
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
            var scriptDetails1 = new ServerScriptDetails(
                "script-1-file-name",
                "script-1-file-path",
                "script-1-string"
            );

            var disposableBasedScript = new DisposableBasedScript();

            var loggerMock = new Mock<ILogger<LoadNewServerScriptAssemblyCommandHandler>>();
            var serverInfoMock = new Mock<ServerInfo>();
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();
            var scriptRepositoryMock = new Mock<ServerScriptRepository>();
            var observerStateMock = new Mock<ObserverState>();
            var serverScriptServicesMock = new Mock<ServerScriptServices>();

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

            serverScriptServicesMock.Setup(
                mock => mock.Logger<It.IsAnyType>()
            ).Returns(
                loggerMock.Object
            );

            // When
            var handler = new LoadNewServerScriptAssemblyCommandHandler(
                loggerMock.Object,
                serverInfoMock.Object,
                detailsRepositoryMock.Object,
                scriptRepositoryMock.Object,
                observerStateMock.Object,
                serverScriptServicesMock.Object
            );
            var actual = await handler.Handle(
                new LoadNewServerScriptAssemblyCommand(),
                CancellationToken.None
            );

            // Then
            disposableBasedScript.DisposedCalled
                .Should().Be(1);
        }

        [Fact]
        public async Task ShouldReturnErrorCodeWhenAnyExceptionIsThrown()
        {
            // Given
            var expected = "SERVER_SCRIPT_FAILED_LOADING_ASSEMBLY_ERROR_CODE";

            var loggerMock = new Mock<ILogger<LoadNewServerScriptAssemblyCommandHandler>>();
            var serverInfoMock = new Mock<ServerInfo>();
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();
            var scriptRepositoryMock = new Mock<ServerScriptRepository>();
            var observerStateMock = new Mock<ObserverState>();
            var serverScriptServicesMock = new Mock<ServerScriptServices>();

            // When
            var handler = new LoadNewServerScriptAssemblyCommandHandler(
                loggerMock.Object,
                serverInfoMock.Object,
                detailsRepositoryMock.Object,
                scriptRepositoryMock.Object,
                observerStateMock.Object,
                serverScriptServicesMock.Object
            );
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
}
