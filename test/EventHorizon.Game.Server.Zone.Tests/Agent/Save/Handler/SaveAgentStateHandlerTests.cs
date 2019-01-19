using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Core.Json;
using EventHorizon.Game.Server.Zone.State.Repository;
using Microsoft.AspNetCore.Hosting;
using EventHorizon.Performance;
using EventHorizon.Game.Server.Zone.Agent.Save.Handler;
using EventHorizon.Game.Server.Zone.Agent.Save;
using System.Threading.Tasks;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Model;
using IOPath = System.IO.Path;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Load.Settings.Model;
using EventHorizon.Game.Server.Zone.Agent.Connection;
using EventHorizon.Game.Server.Zone.Agent;
using EventHorizon.Game.Server.Zone.External.Json;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Save.Handler
{
    public class SaveAgentStateHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldSuccessfullyCallJsonFileSaver()
        {
            // Given
            var inputId1 = 123;
            var inputId2 = 321;
            var inputAgent1 = new AgentEntity
            {
                Id = inputId1,
                RawData = new Dictionary<string, object>(),
            };
            var inputAgent2 = new AgentEntity
            {
                Id = inputId2,
                IsGlobal = true,
                RawData = new Dictionary<string, object>(),
            };
            var inputAgentList = new List<AgentEntity>
            {
                inputAgent1,
                inputAgent2,
            };
            var expectedContentRootPath = IOPath.Combine("some", "content", "path");
            var expectedDirectory = IOPath.Combine(expectedContentRootPath, "App_Data");
            var expectedFileName = "Agent.state.json";

            var zoneSettingsMock = new Mock<ZoneSettings>();
            var jsonFileSaverMock = new Mock<IJsonFileSaver>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
            var performanceTrackerMock = new Mock<IPerformanceTracker>();
            var agentConnectionMock = new Mock<IAgentConnection>();

            hostingEnvironmentMock.Setup(hostingEnvironment => hostingEnvironment.ContentRootPath).Returns(expectedContentRootPath);
            agentRepositoryMock.Setup(agentRepository => agentRepository.All()).ReturnsAsync(inputAgentList);

            // When
            var saveAgentStateHandler = new SaveAgentStateHandler(
                zoneSettingsMock.Object,
                jsonFileSaverMock.Object,
                agentRepositoryMock.Object,
                hostingEnvironmentMock.Object,
                agentConnectionMock.Object,
                performanceTrackerMock.Object
            );

            await saveAgentStateHandler.Handle(new SaveAgentStateEvent(), CancellationToken.None);

            // Then
            agentConnectionMock.Verify(a => a.SendAction("UpdateAgent", It.IsAny<AgentDetails>()));
            agentRepositoryMock.Verify(agentRepository => agentRepository.All());
            hostingEnvironmentMock.Verify(hostingEnvironment => hostingEnvironment.ContentRootPath);
            jsonFileSaverMock.Verify(jsonFileSaver => jsonFileSaver.SaveToFile(expectedDirectory, expectedFileName, It.IsAny<AgentSaveState>()));
        }
    }
}