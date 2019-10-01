using Xunit;
using Moq;
using EventHorizon.Zone.System.Agent.Model;
using System.Numerics;
using EventHorizon.Zone.Core.Model.Core;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Connection.Model;
using EventHorizon.Zone.System.Agent.Save.Mapper;

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
            var inputAgent = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Name = expectedName,
                Position = expectedPosition,
                RawData = new Dictionary<string, object>()
            };
            var expectedDetails = new AgentDetails
            {
                Name = expectedName,
                Position = expectedPosition,
                Data = new Dictionary<string, object>()
            };

            // When
            var actual = AgentFromEntityToDetails.Map(inputAgent);

            // Then
            Assert.Equal(expectedName, actual.Name);
            Assert.Equal(expectedPosition, actual.Position);
            Assert.Null(actual.TagList);
            Assert.Empty(actual.Data);
        }
    }
}