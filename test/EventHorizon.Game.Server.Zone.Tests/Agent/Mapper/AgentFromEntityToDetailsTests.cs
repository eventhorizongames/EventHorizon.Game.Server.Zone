using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Core.Model;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Agent.Mapper;
using EventHorizon.Game.Server.Zone.Model.Core;
using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Mapper
{
    public class AgentFromEntityToDetailsTests
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
            var expectedSpeed = 123;
            var inputAgent = new AgentEntity
            {
                Name = expectedName,
                Position = expectedPosition,
                Speed = expectedSpeed,
                Data = new Dictionary<string, object>()
            };
            var expectedDetails = new AgentDetails
            {
                Name = expectedName,
                Position = expectedPosition,
                Speed = expectedSpeed,
                Data = new Dictionary<string, object>()
            };

            // When
            var actual = AgentFromEntityToDetails.Map(inputAgent);

            // Then
            Assert.Equal(expectedName, actual.Name);
            Assert.Equal(expectedPosition, actual.Position);
            Assert.Equal(expectedSpeed, actual.Speed);
            Assert.Null(actual.TagList);
            Assert.Empty(actual.Data);
        }
    }
}