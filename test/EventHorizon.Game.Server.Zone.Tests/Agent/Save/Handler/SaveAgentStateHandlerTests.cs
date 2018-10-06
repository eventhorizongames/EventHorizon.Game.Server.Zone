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

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Save.Handler
{
    public class SaveAgentStateHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldSuccessfullyCallJsonFileSaver()
        {
            // Given
            var inputId1 = 123;
            var inputAgent1 = new AgentEntity
            {
                Id = inputId1,
                RawData = new Dictionary<string, object>(),
            };
            var inputAgentList = new List<AgentEntity>
            {
                inputAgent1,
            };
            var expectedContentRootPath = IOPath.Combine("some", "content", "path");
            var expectedDirectory = IOPath.Combine(expectedContentRootPath, "App_Data");
            var expectedFileName = "Agent.state.json";

            var jsonFileSaverMock = new Mock<IJsonFileSaver>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            var hostingEnvironmentMock = new Mock<IHostingEnvironment>();

            hostingEnvironmentMock.Setup(hostingEnvironment => hostingEnvironment.ContentRootPath).Returns(expectedContentRootPath);
            agentRepositoryMock.Setup(agentRepository => agentRepository.All()).ReturnsAsync(inputAgentList);

            // When
            var saveAgentStateHandler = new SaveAgentStateHandler(
                jsonFileSaverMock.Object,
                agentRepositoryMock.Object,
                hostingEnvironmentMock.Object
            );

            await saveAgentStateHandler.Handle(new SaveAgentStateEvent(), CancellationToken.None);

            // Then
            hostingEnvironmentMock.Verify(hostingEnvironment => hostingEnvironment.ContentRootPath);
            agentRepositoryMock.Verify(agentRepository => agentRepository.All());
            jsonFileSaverMock.Verify(jsonFileSaver => jsonFileSaver.SaveToFile(expectedDirectory, expectedFileName, It.IsAny<AgentSaveState>()));
        }
    }
}