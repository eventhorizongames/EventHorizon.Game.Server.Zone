namespace EventHorizon.Game.Server.Zone.Tests.Agent.Startup.Handler
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.Core.Model.Settings;
    using EventHorizon.Zone.System.Agent.Connection;
    using EventHorizon.Zone.System.Agent.Connection.Model;
    using EventHorizon.Zone.System.Agent.Events.Register;
    using EventHorizon.Zone.System.Agent.Events.Startup;
    using EventHorizon.Zone.System.Agent.Save.Model;
    using EventHorizon.Zone.System.Agent.Startup;

    using MediatR;

    using Moq;

    using Xunit;

    using IOPath = System.IO.Path;

    public class LoadZoneAgentStateHandlerTests
    {
        [Fact]
        public async Task TestShouldCorrectlyRegisterExpectedAgents()
        {
            // Given
            var expectedAppDataPath = "some-content-path/App_Data";
            var expectedAgentFileName = IOPath.Combine(
                expectedAppDataPath,
                "Agent",
                "Agent.state.json"
            );
            var expectedTag = "server-tag";
            var expectedAgentDetails1 = new AgentDetails
            {
                Transform = new TransformState(),
            };
            var expectedAgentDetails2 = new AgentDetails
            {
                Transform = new TransformState(),
            };
            var expectedAgentList = new List<AgentDetails>()
            {
                expectedAgentDetails1,
                expectedAgentDetails2
            };
            var expectedAgentState = new AgentSaveState
            {
                AgentList = expectedAgentList
            };

            var zoneSettings = new ZoneSettings
            {
                Tag = expectedTag
            };

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var agentConnectionMock = new Mock<IAgentConnection>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            agentConnectionMock.Setup(
                mock => mock.SendAction<IList<AgentDetails>>(
                    "GetAgentsByZoneTag",
                    expectedTag
                )
            ).ReturnsAsync(
                expectedAgentList
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                expectedAppDataPath
            );
            fileLoaderMock.Setup(
                mock => mock.GetFile<AgentSaveState>(
                    expectedAgentFileName
                )
            ).ReturnsAsync(
                expectedAgentState
            );

            // When
            var loadZoneAgentStateHandler = new LoadZoneAgentStateHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                zoneSettings,
                agentConnectionMock.Object,
                fileLoaderMock.Object
            );

            await loadZoneAgentStateHandler.Handle(
                new LoadZoneAgentStateEvent(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<RegisterAgentEvent>(),
                    CancellationToken.None
                ),
                Times.Exactly(
                    4
                )
            );
        }

        [Fact]
        public async Task ShouldNotThrowExceptionWhenFileNotFound()
        {
            // Given
            var appDataPath = "some-content-path/App_Data";
            var agentFileName = IOPath.Combine(
                appDataPath,
                "Agent",
                "Agent.state.json"
            );
            var tag = "server-tag";

            var zoneSettings = new ZoneSettings
            {
                Tag = tag
            };

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var agentConnectionMock = new Mock<IAgentConnection>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();

            agentConnectionMock.Setup(
                mock => mock.SendAction<IList<AgentDetails>>(
                    "GetAgentsByZoneTag",
                    tag
                )
            ).ReturnsAsync(
                new List<AgentDetails>()
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );
            fileLoaderMock.Setup(
                mock => mock.GetFile<AgentSaveState>(
                    agentFileName
                )
            ).ReturnsAsync(
                default(AgentSaveState)
            );

            // When
            var loadZoneAgentStateHandler = new LoadZoneAgentStateHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                zoneSettings,
                agentConnectionMock.Object,
                fileLoaderMock.Object
            );

            await loadZoneAgentStateHandler.Handle(
                new LoadZoneAgentStateEvent(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<RegisterAgentEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
    }
}
