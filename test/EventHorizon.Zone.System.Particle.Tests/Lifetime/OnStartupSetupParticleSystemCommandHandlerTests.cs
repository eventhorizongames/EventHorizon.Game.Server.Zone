namespace EventHorizon.Zone.System.Particle.Tests.Lifetime
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using EventHorizon.Zone.System.Particle.Lifetime;

    using FluentAssertions;

    using global::System.Collections;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class OnStartupSetupParticleSystemCommandHandlerTests
    {
        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public async Task ShouldWriteResourceFileWhenHandleIsCalled(
            ScenarioTestData testData
        )
        {
            // Given
            var expectedResourceRoot = "EventHorizon.Zone.System.Particle";
            var clientPath = "client-path";

            var loggerMock = new Mock<ILogger<OnStartupSetupParticleSystemCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
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
            var handler = new OnStartupSetupParticleSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupParticleSystemCommand(),
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
            var clientPath = "client-path";

            var loggerMock = new Mock<ILogger<OnStartupSetupParticleSystemCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
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

            // When
            var handler = new OnStartupSetupParticleSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupParticleSystemCommand(),
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
            private readonly List<object[]> _data = new()
            {
                new object[]
                {
                    new ScenarioTestData
                    {
                        ExpectedResourcePath = "App_Data.Client.Particle",
                        ExpectedResourceFile = "Flame.json",
                        ExpectedFileFullName = Path.Combine(
                            "client-path",
                            "Particle",
                            "Flame.json"
                        ),
                    }
                },
                new object[]
                {
                    new ScenarioTestData
                    {
                        ExpectedResourcePath = "App_Data.Client.Particle",
                        ExpectedResourceFile = "SelectedCompanionIndicator.json",
                        ExpectedFileFullName = Path.Combine(
                            "client-path",
                            "Particle",
                            "SelectedCompanionIndicator.json"
                        ),
                    }
                },
                new object[]
                {
                    new ScenarioTestData
                    {
                        ExpectedResourcePath = "App_Data.Client.Particle",
                        ExpectedResourceFile = "SelectedIndicator.json",
                        ExpectedFileFullName = Path.Combine(
                            "client-path",
                            "Particle",
                            "SelectedIndicator.json"
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
