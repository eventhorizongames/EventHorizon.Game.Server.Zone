using Xunit;
using Moq;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Model;
using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.State.Impl;
using EventHorizon.Zone.Core.Model.Entity.State;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.State.Impl
{
    public class AgentWrappedEntityRepositoryTests
    {
        [Fact]
        public async Task TestAll_ShouldReturnAllAgentTypeEntitiesFromEntityRepository()
        {
            // Given
            var expectedAgent1 = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 1,
                Type = EntityType.AGENT
            };
            var expectedAgent2 = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 2,
                Type = EntityType.AGENT
            };
            var expectedAgent3 = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 3,
                Type = EntityType.AGENT
            };
            var otherEntity1 = new AgentEntity(
                new Dictionary<string, object>()
            )
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

            var entityRepositoryMock = new Mock<EntityRepository>();

            entityRepositoryMock.Setup(
                mock => mock.All()
            ).ReturnsAsync(
                expectedEntityList
            );

            // When
            var agentRepository = new AgentWrappedEntityRepository(
                entityRepositoryMock.Object
            );

            var actual = await agentRepository.All();

            // Then
            Assert.Collection(actual,
                entity => Assert.Equal(
                    expectedAgent1,
                    entity
                ),
                entity => Assert.Equal(
                    expectedAgent2,
                    entity
                ),
                entity => Assert.Equal(
                    expectedAgent3,
                    entity
                )
            );
        }
        [Fact]
        public async Task TestAll_ShouldReturnNoAgentsWhenEntityRepositoryDoesNotContainAgentEntity()
        {
            // Given
            var otherEntity1 = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 9999,
                Type = EntityType.PLAYER
            };

            var expectedEntityList = new List<IObjectEntity>()
            {
                otherEntity1,
            };

            var entityRepositoryMock = new Mock<EntityRepository>();

            entityRepositoryMock.Setup(
                mock => mock.All()
            ).ReturnsAsync(
                expectedEntityList
            );

            // When
            var agentRepository = new AgentWrappedEntityRepository(
                entityRepositoryMock.Object
            );

            var actual = await agentRepository.All();

            // Then
            Assert.Empty(
                actual
            );
        }
        [Fact]
        public async Task TestFindById_ShouldReturnAgentWithPassedId()
        {
            // Given
            var inputId = 1L;
            var expectedAgent = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = inputId,
                Type = EntityType.AGENT
            };
            var otherAgent1 = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 2,
                Type = EntityType.AGENT
            };
            var otherAgent2 = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 3,
                Type = EntityType.AGENT
            };
            var otherEntity1 = new AgentEntity(
                new Dictionary<string, object>()
            )
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

            var entityRepositoryMock = new Mock<EntityRepository>();

            entityRepositoryMock.Setup(
                mock => mock.All()
            ).ReturnsAsync(
                expectedEntityList
            );

            // When
            var agentRepository = new AgentWrappedEntityRepository(
                entityRepositoryMock.Object
            );

            var actual = await agentRepository.FindById(
                inputId
            );

            // Then
            Assert.Equal(
                expectedAgent, 
                actual
            );
        }
        [Fact]
        public async Task TestFindById_ShouldReturnDefaultAgentWhenPassedIdIsNotFound()
        {
            // Given
            var inputId = 1L;
            var otherAgent1 = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 2,
                Type = EntityType.AGENT
            };
            var otherAgent2 = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 3,
                Type = EntityType.AGENT
            };
            var otherEntity1 = new AgentEntity(
                new Dictionary<string, object>()
            )
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

            var entityRepositoryMock = new Mock<EntityRepository>();

            entityRepositoryMock.Setup(
                mock => mock.All()
            ).ReturnsAsync(
                expectedEntityList
            );

            // When
            var agentRepository = new AgentWrappedEntityRepository(
                entityRepositoryMock.Object
            );

            var actual = await agentRepository.FindById(inputId);

            // Then
            Assert.False(
                actual.IsFound()
            );
            Assert.Equal(
                default(AgentEntity), 
                actual
            );
        }
        [Fact]
        public async Task TestFUpdate_ShouldPassParametersToEntityRepository()
        {
            // Given
            var expectedEntityAction = EntityAction.POSITION;
            var expectedAgent = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 3,
                Type = EntityType.AGENT
            };
            var entityRepositoryMock = new Mock<EntityRepository>();

            // When
            var agentRepository = new AgentWrappedEntityRepository(
                entityRepositoryMock.Object
            );

            await agentRepository.Update(
                expectedEntityAction, 
                expectedAgent
            );

            // Then
            entityRepositoryMock.Verify(
                mock => mock.Update(
                    expectedEntityAction, 
                    expectedAgent
                )
            );
        }
    }
}