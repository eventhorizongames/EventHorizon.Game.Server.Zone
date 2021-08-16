namespace EventHorizon.Game.Server.Zone.Tests.Agent.Save.Handler
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Performance;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Agent.Connection;
    using EventHorizon.Zone.System.Agent.Connection.Model;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Model.State;
    using EventHorizon.Zone.System.Agent.Save;
    using EventHorizon.Zone.System.Agent.Save.Events;
    using EventHorizon.Zone.System.Agent.Save.Model;

    using Moq;

    using Xunit;

    public class SaveAgentStateHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldSuccessfullyCallJsonFileSaver()
        {
            // Given
            var inputId1 = 123;
            var inputId2 = 321;
            var inputAgent1 = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = inputId1,
            };
            var inputAgent2 = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = inputId2,
                IsGlobal = true,
            };
            var inputAgentList = new List<AgentEntity>
            {
                inputAgent1,
                inputAgent2,
            };
            var expectedAppDataPath = Path.Combine(
                "some",
                "content",
                "path",
                "App_Data"
            );
            var expectedDirectory = Path.Combine(
                expectedAppDataPath,
                "Agent"
            );
            var expectedFileName = "Agent.state.json";

            var serverInfoMock = new Mock<ServerInfo>();
            var jsonFileSaverMock = new Mock<IJsonFileSaver>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            var performanceTrackerFactoryMock = new Mock<PerformanceTrackerFactory>();
            var agentConnectionMock = new Mock<IAgentConnection>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                expectedAppDataPath
            );
            agentRepositoryMock.Setup(
                agentRepository => agentRepository.All()
            ).ReturnsAsync(
                inputAgentList
            );

            // When
            var saveAgentStateHandler = new SaveAgentStateHandler(
                serverInfoMock.Object,
                jsonFileSaverMock.Object,
                agentRepositoryMock.Object,
                agentConnectionMock.Object,
                performanceTrackerFactoryMock.Object
            );

            await saveAgentStateHandler.Handle(
                new SaveAgentStateEvent(),
                CancellationToken.None
            );

            // Then
            agentConnectionMock.Verify(
                mock => mock.SendAction(
                    "UpdateAgent",
                    It.IsAny<AgentDetails>()
                )
            );
            agentRepositoryMock.Verify(
                mock => mock.All()
            );
            serverInfoMock.Verify(
                mock => mock.AppDataPath
            );
            jsonFileSaverMock.Verify(
                mock => mock.SaveToFile(
                    expectedDirectory,
                    expectedFileName,
                    It.IsAny<AgentSaveState>()
                )
            );
        }
    }
}
