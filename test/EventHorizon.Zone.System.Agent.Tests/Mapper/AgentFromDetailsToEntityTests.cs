using Xunit;
using Moq;
using System.Numerics;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Connection.Model;
using EventHorizon.Zone.System.Agent.Save.Mapper;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Mapper
{
    public class AgentFromDetailsToEntityTests
    {
        [Fact]
        public void TestMapToNew_ShouldReturnAgentEntityFilledFromDetails()
        {
            // Given
            var inputAgent = new AgentDetails
            {
                Name = "Agent 001",
                Transform = new TransformState
                {
                    Position = Vector3.Zero,
                },
                Data = new Dictionary<string, object>(),
            };
            var agentId = "test-id";
            var expectedId = -1;
            var expectedType = EntityType.AGENT;
            var expectedName = inputAgent.Name;
            var expectedPosition = inputAgent.Transform.Position;
            var expectedData = inputAgent.Data;

            // When
            var actual = AgentFromDetailsToEntity.MapToNew(inputAgent, agentId);

            // Then
            var actualLocationState = actual.GetProperty<LocationState>(
                LocationState.PROPERTY_NAME
            );
            Assert.Equal(expectedId, actual.Id);
            Assert.Equal(expectedType, actual.Type);
            Assert.Equal(expectedName, actual.Name);
            Assert.Equal(expectedPosition, actual.Transform.Position);
            Assert.Equal(expectedData, actual.Data);
        }
    }
}