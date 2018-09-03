using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Mapper;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Core.Model;
using System;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Core.Dynamic;

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
                Data = new NullingExpandoObject(),
                Ai = new AgentAiDetails(),
                Speed = 1
            };
            var expectedId = -1;
            var expectedType = EntityType.AGENT;
            var expectedName = inputAgent.Name;
            var expectedCurrentPosition = inputAgent.Position.CurrentPosition;
            var expectedCurrentZone = inputAgent.Position.CurrentZone;
            var expectedZoneTag = inputAgent.Position.ZoneTag;

            var expectedMoveToPosition = inputAgent.Position.CurrentPosition;
            var expectedData = inputAgent.Data;
            var expectedAi = inputAgent.Ai;
            var expectedSpeed = inputAgent.Speed;

            // When
            var actual = AgentFromDetailsToEntity.MapToNew(inputAgent);

            // Then
            Assert.Equal(expectedId, actual.Id);
            Assert.Equal(expectedType, actual.Type);
            Assert.Equal(expectedName, actual.Name);
            Assert.Equal(expectedCurrentPosition, actual.Position.CurrentPosition);
            Assert.Equal(expectedCurrentZone, actual.Position.CurrentZone);
            Assert.Equal(expectedZoneTag, actual.Position.ZoneTag);
            Assert.Equal(expectedMoveToPosition, actual.Position.MoveToPosition);
            Assert.Equal(expectedData, actual.Data);
            Assert.Equal(expectedAi, actual.Ai);
            Assert.Equal(expectedSpeed, actual.Speed);
        }
    }
}