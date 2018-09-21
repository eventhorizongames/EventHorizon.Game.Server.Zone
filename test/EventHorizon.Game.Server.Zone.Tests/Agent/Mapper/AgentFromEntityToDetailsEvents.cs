using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Core.Model;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Core.Dynamic;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Agent.Mapper;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Mapper
{
    public class AgentFromEntityToDetailsEvents
    {
        [Fact]
        public void TestMapToNew_ShouldReturnAgentDetailsFilledFromEntity()
        {
            // Given
            var expectedName = "Agent 001";
            var expectedPosition = new PositionState
            {
                CurrentPosition = Vector3.Zero,
                CurrentZone = "current-zone",
                ZoneTag = "test"
            };
            var expectedData = new NullingExpandoObject();
            var expectedAi = new AgentAiState();
            var expectedSpeed = 123;
            var inputAgent = new AgentEntity
            {
                Name = expectedName,
                Position = expectedPosition,
                Data = expectedData,
                Ai = expectedAi,
                Speed = expectedSpeed
            };
            var expectedDetails = new AgentDetails
            {
                Name = expectedName,
                Position = expectedPosition,
                Data = expectedData,
                Ai = expectedAi,
                Speed = expectedSpeed
            };

            // When
            var actual = AgentFromEntityToDetails.Map(inputAgent);

            // Then
            Assert.Equal(expectedDetails, actual);
        }
    }
}