using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository.Impl;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Respository.Impl
{
    public class MoveAgentRepositoryTests
    {
        [Fact]
        public void Test_ShouldAddAgentEntityIdToAllList()
        {
            // Given
            var inputId1 = 123;

            // When
            var moveAgentRepository = new MoveAgentRepository();
            moveAgentRepository.Add(inputId1);

            // Then
            var list = moveAgentRepository.All();

            Assert.Collection(list,
                entityId => Assert.Equal(inputId1, entityId)
            );
        }
        [Fact]
        public void Test_ShouldRemoveAgentEntityIdFromAllList()
        {
            // Given
            var inputId1 = 123;

            // When
            var moveAgentRepository = new MoveAgentRepository();
            moveAgentRepository.Add(inputId1);
            var list = moveAgentRepository.All();

            Assert.Collection(list,
                entityId => Assert.Equal(inputId1, entityId)
            );

            // Then
            moveAgentRepository.Remove(inputId1);
            list = moveAgentRepository.All();

            Assert.Empty(list);
        }
    }
}