namespace EventHorizon.Game.Server.Zone.Tests.Agent.Mapper
{
    using Xunit;
    using EventHorizon.Zone.System.Agent.Model;
    using System.Numerics;
    using EventHorizon.Zone.Core.Model.Core;
    using System.Collections.Generic;
    using EventHorizon.Zone.System.Agent.Save.Mapper;
    using EventHorizon.Zone.Core.Model.Entity;

    public class AgentFromEntityToDetailsTests
    {
        [Fact]
        public void TestMapToNew_ShouldReturnAgentDetailsFilledFromEntity()
        {
            // Given
            var expectedName = "Agent 001";
            var expectedTransform = new TransformState
            {
                Position = Vector3.Zero,
            };
            var expectedLocation = new LocationState
            {
                CurrentZone = "current-zone",
                ZoneTag = "test"
            };
            var inputAgent = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Name = expectedName,
                Transform = new TransformState
                {
                    Position = Vector3.Zero,
                },
            };
            inputAgent.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME,
                expectedLocation
            );

            // When
            var actual = AgentFromEntityToDetails.Map(inputAgent);

            // Then
            Assert.Equal(expectedName, actual.Name);
            Assert.Equal(expectedTransform, actual.Transform);
            Assert.Equal(expectedLocation, actual.Location);
            Assert.Null(actual.TagList);
            Assert.Collection(
                actual.Data,
                actualData => Assert.Equal(
                    actualData.Value,
                    expectedLocation
                )
            );
        }
    }
}