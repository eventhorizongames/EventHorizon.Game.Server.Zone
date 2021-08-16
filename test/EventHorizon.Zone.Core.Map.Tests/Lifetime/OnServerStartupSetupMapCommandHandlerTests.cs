namespace EventHorizon.Zone.Core.Map.Tests.Lifetime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Map.Lifetime;
    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.Core.Model.Lifetime;

    using FluentAssertions;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class OnServerStartupSetupMapCommandHandlerTests
    {
        [Fact]
        public async Task ShouldCreateDefaultSettingsWhenFileDoesNotExist()
        {
            // Given
            var adminPath = "admin-path";
            var serverScriptsPath = "scripts-path";
            var directory = $"{Path.DirectorySeparatorChar}some-file-directory";
            var fileName = "Map.state.json";
            var expected = DefaultMapSettings.DEFAULT_MAP;

            var loggerMock = new Mock<ILogger<OnServerStartupSetupMapCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileSaverMock = new Mock<IJsonFileSaver>();

            serverInfoMock.Setup(
                mock => mock.CoreMapPath
            ).Returns(
                directory
            );
            serverInfoMock.Setup(
                mock => mock.AdminPath
            ).Returns(
                adminPath
            );
            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<WriteResourceToFile>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new StandardCommandResult()
            );

            // When
            var handler = new OnServerStartupSetupMapCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                fileSaverMock.Object
            );
            var actual = await handler.Handle(
                new OnServerStartupSetupMapCommand(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                new OnServerStartupResult(
                    true
                )
            );

            fileSaverMock.Verify(
                mock => mock.SaveToFile(
                    directory,
                    fileName,
                    expected
                )
            );
        }

        [Fact]
        public async Task ShouldNotCreateDefaultSettingsWhenFileDoesExist()
        {
            // Given
            var adminPath = "admin-path";
            var serverScriptsPath = "scripts-path";
            var directory = $"{Path.DirectorySeparatorChar}some-file-directory";
            var fileName = "Map.state.json";
            var fileFullName = Path.Combine(
                directory,
                fileName
            );

            var loggerMock = new Mock<ILogger<OnServerStartupSetupMapCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileSaverMock = new Mock<IJsonFileSaver>();

            serverInfoMock.Setup(
                mock => mock.CoreMapPath
            ).Returns(
                directory
            );
            serverInfoMock.Setup(
                mock => mock.AdminPath
            ).Returns(
                adminPath
            );
            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<WriteResourceToFile>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new StandardCommandResult()
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesFileExist(
                        fileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var handler = new OnServerStartupSetupMapCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                fileSaverMock.Object
            );
            var actual = await handler.Handle(
                new OnServerStartupSetupMapCommand(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                new OnServerStartupResult(
                    true
                )
            );

            fileSaverMock.Verify(
                mock => mock.SaveToFile(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<ZoneMapDetails>()
                ),
                Times.Never()
            );
        }

        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public async Task ShouldWriteResourceFileWhenHandleIsCalled(
            ScenarioTestData testData
        )
        {
            // Given
            var expectedResourceRoot = "EventHorizon.Zone.Core.Map";
            var adminPath = "admin-path";
            var serverScriptsPath = "server-scripts-path";
            var coreMapPath = "core-map-path";

            var loggerMock = new Mock<ILogger<OnServerStartupSetupMapCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileSaverMock = new Mock<IJsonFileSaver>();

            serverInfoMock.Setup(
                mock => mock.CoreMapPath
            ).Returns(
                coreMapPath
            );
            serverInfoMock.Setup(
                mock => mock.AdminPath
            ).Returns(
                adminPath
            );
            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<WriteResourceToFile>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new StandardCommandResult()
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<DoesFileExist>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var handler = new OnServerStartupSetupMapCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                fileSaverMock.Object
            );
            var actual = await handler.Handle(
                new OnServerStartupSetupMapCommand(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                new OnServerStartupResult(
                    true
                )
            );

            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<WriteResourceToFile>(
                        a => a.ResourceRoot == expectedResourceRoot
                            && a.ResourcePath == testData.ExpectedResourcePath
                            && a.ResourceFile == testData.ExpectedResourceFile
                            && a.SaveFileFullName == testData.ExpectedFileFullName
                    ),
                    CancellationToken.None
                )
            );
        }

        [Theory]
        [InlineData("file_already_exists")]
        [InlineData("some_random_error_code")]
        public async Task ShouldNotThrowExceptionOnErrorCodesFromWriteResourceFile(
            string errorCode
        )
        {
            // Given
            var adminPath = "admin-path";
            var serverScriptsPath = "server-scripts-path";
            var coreMapPath = "core-map-path";

            var loggerMock = new Mock<ILogger<OnServerStartupSetupMapCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileSaverMock = new Mock<IJsonFileSaver>();

            serverInfoMock.Setup(
                mock => mock.CoreMapPath
            ).Returns(
                coreMapPath
            );
            serverInfoMock.Setup(
                mock => mock.AdminPath
            ).Returns(
                adminPath
            );
            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<WriteResourceToFile>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new StandardCommandResult(
                    errorCode
                )
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<DoesFileExist>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var handler = new OnServerStartupSetupMapCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                fileSaverMock.Object
            );
            var actual = await handler.Handle(
                new OnServerStartupSetupMapCommand(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                new OnServerStartupResult(
                    true
                )
            );
        }

        public class TestDataGenerator
            : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[]
                {
                    new ScenarioTestData
                    {
                        ExpectedResourcePath = "App_Data.Admin.Commands",
                        ExpectedResourceFile = "ReloadCoreMap.json",
                        ExpectedFileFullName = Path.Combine(
                            "admin-path",
                            "Commands",
                            "ReloadCoreMap.json"
                        ),
                    }
                },
                new object[]
                {
                    new ScenarioTestData
                    {
                        ExpectedResourcePath = "App_Data.Admin.Commands",
                        ExpectedResourceFile = "ReloadCoreMap_cmd.json",
                        ExpectedFileFullName = Path.Combine(
                            "admin-path",
                            "Commands",
                            "ReloadCoreMap_cmd.json"
                        ),
                    }
                },
                new object[]
                {
                    new ScenarioTestData
                    {
                        ExpectedResourcePath = "App_Data.Server.Scripts.Admin.Map",
                        ExpectedResourceFile = "ReloadCoreMap.csx",
                        ExpectedFileFullName = Path.Combine(
                            "server-scripts-path",
                            "Admin",
                            "Map",
                            "ReloadCoreMap.csx"
                        ),
                    }
                },
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class ScenarioTestData
        {
            public string ExpectedResourcePath { get; set; }
            public string ExpectedResourceFile { get; set; }
            public string ExpectedFileFullName { get; set; }
        }
    }
}
