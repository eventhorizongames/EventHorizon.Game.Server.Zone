using Xunit;
using Moq;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using IOPath = System.IO.Path;
using System.Threading;
using EventHorizon.Zone.System.Agent.Startup;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Agent.Connection;
using EventHorizon.Zone.System.Agent.Connection.Model;
using EventHorizon.Zone.System.Agent.Save.Model;
using EventHorizon.Zone.Core.Model.Settings;
using EventHorizon.Zone.System.Agent.Events.Startup;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Agent.Events.Register;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Startup.Handler
{
    public class LoadZoneAgentListHandlerTests
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
                Position = new PositionState()
            };
            var expectedAgentDetails2 = new AgentDetails
            {
                Position = new PositionState()
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
    }
}