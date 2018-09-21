using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository.Impl;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Respository.Impl
{
    public class MoveAgentRepositoryTests
    {
        [Fact]
        public void Test_ShouldAddAgentIdToAllList()
        {
            // Given
            var inputId1 = 123;

            // When
            var moveAgentRepository = new MoveAgentRepository();
            moveAgentRepository.Add(inputId1);

            // Then
            var list = moveAgentRepository.All();

            Assert.Collection(list,
                agentId => Assert.Equal(inputId1, agentId)
            );
        }
        [Fact]
        public void Test_ShouldRemoveAgentIdFromAllList()
        {
            // Given
            var inputId1 = 123;

            // When
            var moveAgentRepository = new MoveAgentRepository();
            moveAgentRepository.Add(inputId1);
            var list = moveAgentRepository.All();

            Assert.Collection(list,
                agentId => Assert.Equal(inputId1, agentId)
            );

            // Then
            moveAgentRepository.Remove(inputId1);
            list = moveAgentRepository.All();

            Assert.Empty(list);
        }
    }
}