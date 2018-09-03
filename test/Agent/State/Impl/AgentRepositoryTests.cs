using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Agent.State.Impl;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;
using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.State.Impl
{
    public class AgentRepositoryTests
    {
        [Fact]
        public async Task TestAll_ShouldReturnAllAgentTypeEntitiesFromEntityRepository()
        {
            // Given
            var expectedAgent1 = new AgentEntity
            {
                Id = 1,
                Type = EntityType.AGENT
            };
            var expectedAgent2 = new AgentEntity
            {
                Id = 2,
                Type = EntityType.AGENT
            };
            var expectedAgent3 = new AgentEntity
            {
                Id = 3,
                Type = EntityType.AGENT
            };
            var otherEntity1 = new AgentEntity
            {
                Id = 9999,
                Type = EntityType.PLAYER
            };

            var expectedEntityList = new List<IObjectEntity>()
            {
                expectedAgent1,
                otherEntity1,
                expectedAgent2,
                expectedAgent3
            };

            var entityRepositoryMock = new Mock<IEntityRepository>();

            entityRepositoryMock.Setup(entityRepository => entityRepository.All()).ReturnsAsync(expectedEntityList);

            // When
            var agentRepository = new AgentRepository(
                entityRepositoryMock.Object
            );

            var actual = await agentRepository.All();

            // Then
            Assert.Collection(actual,
                entity => Assert.Equal(expectedAgent1, entity),
                entity => Assert.Equal(expectedAgent2, entity),
                entity => Assert.Equal(expectedAgent3, entity)
            );
        }
        [Fact]
        public async Task TestAll_ShouldReturnNoAgentsWhenEntityRepositoryDoesNotContainAgentEntity()
        {
            // Given
            var otherEntity1 = new AgentEntity
            {
                Id = 9999,
                Type = EntityType.PLAYER
            };

            var expectedEntityList = new List<IObjectEntity>()
            {
                otherEntity1,
            };

            var entityRepositoryMock = new Mock<IEntityRepository>();

            entityRepositoryMock.Setup(entityRepository => entityRepository.All()).ReturnsAsync(expectedEntityList);

            // When
            var agentRepository = new AgentRepository(
                entityRepositoryMock.Object
            );

            var actual = await agentRepository.All();

            // Then
            Assert.Empty(actual);
        }
        [Fact]
        public async Task TestFindById_ShouldReturnAgentWithPassedId()
        {
            // Given
            var inputId = 1L;
            var expectedAgent = new AgentEntity
            {
                Id = inputId,
                Type = EntityType.AGENT
            };
            var otherAgent1 = new AgentEntity
            {
                Id = 2,
                Type = EntityType.AGENT
            };
            var otherAgent2 = new AgentEntity
            {
                Id = 3,
                Type = EntityType.AGENT
            };
            var otherEntity1 = new AgentEntity
            {
                Id = 9999,
                Type = EntityType.PLAYER
            };

            var expectedEntityList = new List<IObjectEntity>()
            {
                otherEntity1,
                expectedAgent,
                otherAgent1,
                otherAgent2
            };

            var entityRepositoryMock = new Mock<IEntityRepository>();

            entityRepositoryMock.Setup(entityRepository => entityRepository.All()).ReturnsAsync(expectedEntityList);

            // When
            var agentRepository = new AgentRepository(
                entityRepositoryMock.Object
            );

            var actual = await agentRepository.FindById(inputId);

            // Then
            Assert.Equal(expectedAgent, actual);
        }
        [Fact]
        public async Task TestFindById_ShouldReturnDefaultAgentWhenPassedIdIsNotFound()
        {
            // Given
            var inputId = 1L;
            var otherAgent1 = new AgentEntity
            {
                Id = 2,
                Type = EntityType.AGENT
            };
            var otherAgent2 = new AgentEntity
            {
                Id = 3,
                Type = EntityType.AGENT
            };
            var otherEntity1 = new AgentEntity
            {
                Id = 9999,
                Type = EntityType.PLAYER
            };

            var expectedEntityList = new List<IObjectEntity>()
            {
                otherEntity1,
                otherAgent1,
                otherAgent2
            };

            var entityRepositoryMock = new Mock<IEntityRepository>();

            entityRepositoryMock.Setup(entityRepository => entityRepository.All()).ReturnsAsync(expectedEntityList);

            // When
            var agentRepository = new AgentRepository(
                entityRepositoryMock.Object
            );

            var actual = await agentRepository.FindById(inputId);

            // Then
            Assert.False(actual.IsFound());
            Assert.Equal(default(AgentEntity), actual);
        }
        [Fact]
        public async Task TestFUpdate_ShouldPassParametersToEntityRepository()
        {
            // Given
            var expectedEntityAction = EntityAction.POSITION;
            var expectedAgent = new AgentEntity
            {
                Id = 3,
                Type = EntityType.AGENT
            };
            var entityRepositoryMock = new Mock<IEntityRepository>();

            // When
            var agentRepository = new AgentRepository(
                entityRepositoryMock.Object
            );

            await agentRepository.Update(expectedEntityAction, expectedAgent);

            // Then
            entityRepositoryMock.Verify(entityRepository => entityRepository.Update(expectedEntityAction, expectedAgent));
        }
    }
}