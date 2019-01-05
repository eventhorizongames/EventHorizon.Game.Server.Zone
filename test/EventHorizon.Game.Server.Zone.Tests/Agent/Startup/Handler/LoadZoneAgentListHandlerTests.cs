using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using EventHorizon.Game.Server.Zone.Core.Json;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Mapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using IOPath = System.IO.Path;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Register;
using EventHorizon.Game.Server.Zone.Agent.Startup;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Game.Server.Zone.Load.Settings.Model;
using EventHorizon.Game.Server.Zone.Agent.Connection;
using EventHorizon.Game.Server.Zone.Agent;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Startup.Handler
{
    public class LoadZoneAgentListHandlerTests
    {
        [Fact]
        public async Task Test()
        {
            // Given
            var expectedContentRootPath = "some-content-path";
            var expectedAgentFileName = IOPath.Combine(expectedContentRootPath, "App_Data", "Agent.state.json");
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
            var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
            var agentConnectionMock = new Mock<IAgentConnection>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            agentConnectionMock.Setup(a => a.SendAction<IList<AgentDetails>>("GetAgentsByZoneTag", expectedTag)).ReturnsAsync(expectedAgentList);

            hostingEnvironmentMock.Setup(hostingEnvironment => hostingEnvironment.ContentRootPath).Returns(expectedContentRootPath);
            fileLoaderMock.Setup(jsonFileLoader => jsonFileLoader.GetFile<AgentSaveState>(expectedAgentFileName)).ReturnsAsync(expectedAgentState);

            // When
            var loadZoneAgentStateHandler = new LoadZoneAgentStateHandler(
                mediatorMock.Object,
                hostingEnvironmentMock.Object,
                zoneSettings,
                agentConnectionMock.Object,
                fileLoaderMock.Object
            );

            await loadZoneAgentStateHandler.Handle(new LoadZoneAgentStateEvent(), CancellationToken.None);

            // Then
            mediatorMock.Verify(mediator => mediator.Send(It.IsAny<RegisterAgentEvent>(), CancellationToken.None), Times.Exactly(2));
        }
    }
}