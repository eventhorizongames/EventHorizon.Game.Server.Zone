namespace EventHorizon.Game.Server.Zone.Tests.Agent.Mapper
{
    using System.Collections.Concurrent;

    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Connection.Model;
    using EventHorizon.Zone.System.Agent.Save.Mapper;

    using global::System.Numerics;

    using Xunit;

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
                Data = new ConcurrentDictionary<string, object>(),
            };
            var agentId = "test-id";
            var expectedId = -1;
            var expectedType = EntityType.AGENT;
            var expectedName = inputAgent.Name;
            var expectedPosition = Vector3.Zero;

            // When
            var actual = AgentFromDetailsToEntity.MapToNew(inputAgent, agentId);
            var actualLocationState = actual.GetProperty<LocationState>(
                LocationState.PROPERTY_NAME
            );

            // Then
            Assert.Equal(expectedId, actual.Id);
            Assert.Equal(expectedType, actual.Type);
            Assert.Equal(expectedName, actual.Name);
            Assert.Equal(expectedPosition, actual.Transform.Position);
            Assert.Collection(
                actual.Data,
                actualData => Assert.Equal(
                    actualLocationState,
                    (LocationState)actualData.Value
                )
            );
        }
    }
}
