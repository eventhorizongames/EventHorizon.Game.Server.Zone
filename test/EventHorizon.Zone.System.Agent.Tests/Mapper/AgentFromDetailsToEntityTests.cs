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
                Position = new PositionState
                {
                    CurrentPosition = Vector3.Zero,
                    CurrentZone = "current-zone",
                    ZoneTag = "test"
                },
                Data = new Dictionary<string, object>(),
            };
            var agentId = "test-id";
            var expectedId = -1;
            var expectedType = EntityType.AGENT;
            var expectedName = inputAgent.Name;
            var expectedCurrentPosition = inputAgent.Position.CurrentPosition;
            var expectedCurrentZone = inputAgent.Position.CurrentZone;
            var expectedZoneTag = inputAgent.Position.ZoneTag;

            var expectedMoveToPosition = inputAgent.Position.CurrentPosition;
            var expectedData = inputAgent.Data;

            // When
            var actual = AgentFromDetailsToEntity.MapToNew(inputAgent, agentId);

            // Then
            Assert.Equal(expectedId, actual.Id);
            Assert.Equal(expectedType, actual.Type);
            Assert.Equal(expectedName, actual.Name);
            Assert.Equal(expectedCurrentPosition, actual.Position.CurrentPosition);
            Assert.Equal(expectedCurrentZone, actual.Position.CurrentZone);
            Assert.Equal(expectedZoneTag, actual.Position.ZoneTag);
            Assert.Equal(expectedMoveToPosition, actual.Position.MoveToPosition);
            Assert.Equal(expectedData, actual.Data);
        }
    }
}