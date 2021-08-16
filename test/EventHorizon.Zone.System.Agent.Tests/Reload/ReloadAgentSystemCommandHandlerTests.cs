namespace EventHorizon.Zone.System.Agent.Tests.Reload
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Agent.Connection.Model;
    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Events.Register;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Reload;
    using EventHorizon.Zone.System.Agent.Save.Model;

    using FluentAssertions;

    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Text.Json;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;


    public class ReloadAgentSystemCommandHandlerTests
    {
        [Fact]
        public async Task ShouldReturnErrorCodeWhenNoFilesInReloadDirectory()
        {
            // Given
            var appDataPath = "app-data-path";
            var reloadAgentDirectory = Path.Combine(
                appDataPath,
                "Agent",
                "Reload"
            );

            var expected = "no_agents_to_load";

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetListOfFilesFromDirectory(
                        reloadAgentDirectory
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<StandardFileInfo>()
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new ReloadAgentSystemCommandHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new ReloadAgentSystemCommand(),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeFalse();
            actual.ErrorCode.Should().Be(expected);
        }

        [Fact]
        public async Task ShouldRunExpectedCommandsWhenFilesFoundInReloadDirectory()
        {
            // Given
            var appDataPath = "app-data-path";
            var reloadAgentDirectory = Path.Combine(
                appDataPath,
                "Agent",
                "Reload"
            );
            var fileFullName = "file-full-name";
            var file = new StandardFileInfo(
                "file",
                "directory-name",
                fileFullName,
                "exe"
            );
            var agentId = "agent-id-001";
            var agentSaveState = new AgentSaveState
            {
                AgentList = new List<AgentDetails>
                {
                    new AgentDetails
                    {
                        Id = agentId,
                    },
                },
            };
            var agentSaveStateText = JsonSerializer.Serialize(agentSaveState);

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetListOfFilesFromDirectory(
                        reloadAgentDirectory
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<StandardFileInfo>
                {
                    file,
                }
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<GetAgentListEvent>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<AgentEntity>
                {
                    new AgentEntity(
                        new ConcurrentDictionary<string, object>()
                    )
                    {
                        AgentId = agentId,
                    },
                }
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextFromFile(
                        fileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                agentSaveStateText
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new ReloadAgentSystemCommandHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new ReloadAgentSystemCommand(),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();

            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<UnRegisterAgent>(),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<RegisterAgentEvent>(),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new DeleteFile(
                        fileFullName
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
