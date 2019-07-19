using System;
using System.Collections.Generic;
using System.Linq;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.State;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.State
{
    [CleanupInMemoryActorBehaviorTreeRepository]
    public class InMemoryActorBehaviorTreeRepositoryTests
    {
        [Fact]
        public void ShouldReturnEmptyListOfActorIdsForTreeId()
        {
            // Given
            var treeId = "tree-id";

            // When
            var repository = new InMemoryActorBehaviorTreeRepository();

            var actual = repository.ActorIdList(
                treeId
            );

            // Then
            Assert.Empty(
                actual
            );
        }
        [Fact]
        public void ShouldReturnListOfActorIdsAfterAddWhenAddedToRegisteredTree()
        {
            // Given
            var treeId = "tree-id";
            var treeShape = new ActorBehaviorTreeShape();
            var actorId = 1L;
            var expectedActorId = actorId;

            // When
            var repository = new InMemoryActorBehaviorTreeRepository();
            repository.RegisterTree(
                treeId,
                treeShape
            );
            repository.RegisterActorToTree(
                actorId,
                treeId
            );
            var actual = repository.ActorIdList(
                treeId
            );

            // Then
            Assert.Collection(
                actual,
                actualActorId => Assert.Equal(
                    expectedActorId,
                    actualActorId
                )
            );
        }
        [Fact]
        public void ShouldRemoveActorIdFromRegisteredTreeIdWhenContainedInRepository()
        {
            // Given
            var treeId = "tree-id";
            var treeShape = new ActorBehaviorTreeShape();
            var actorId = 1L;
            var expectedActorId = actorId;

            // When
            var repository = new InMemoryActorBehaviorTreeRepository();
            repository.RegisterTree(
                treeId,
                treeShape
            );
            repository.RegisterActorToTree(
                actorId,
                treeId
            );
            var initialState = repository.ActorIdList(
                treeId
            );

            // Validate Expected Initial State
            Assert.Collection(
                initialState,
                actualActorId => Assert.Equal(
                    expectedActorId,
                    actualActorId
                )
            );
            repository.UnRegisterActorFromTree(
                actorId,
                treeId
            );

            var actual = repository.ActorIdList(
                treeId
            );

            // Then
            Assert.Empty(
                actual
            );
        }
        [Fact]
        public void ShouldReturnRegisteredTreeWhenRegistered()
        {
            // Given
            var expected = new BehaviorNode();
            var treeId = "tree-id";
            var treeShape = new ActorBehaviorTreeShape
            {
                NodeList = new List<BehaviorNode>()
                {
                    expected
                }
            };

            // When
            var repository = new InMemoryActorBehaviorTreeRepository();

            // Validate Expected Initial State
            var initialState = repository.FindTreeShape(
                treeId
            );
            Assert.Empty(
                initialState.NodeList
            );
            repository.RegisterTree(
                treeId,
                treeShape
            );

            var actual = repository.FindTreeShape(
                treeId
            );

            // Then
            Assert.Collection(
                actual.NodeList,
                node => Assert.Equal(
                    expected,
                    node
                )
            );
        }
        [Fact]
        public void ShouldMergeTheExistingActorListWhenRegisteringNewTree()
        {
            // Given
            var actorId1 = 123L;
            var actorId2 = 321L;
            var expectedActorId1 = actorId1;
            var expectedActorId2 = actorId2;
            var expectedNode1 = new BehaviorNode(
                new SerializedBehaviorNode()
            );
            var expectedNode2 = new BehaviorNode(
                new SerializedBehaviorNode()
            );
            var treeId = "tree-id";
            var treeShape1 = new ActorBehaviorTreeShape
            {
                NodeList = new List<BehaviorNode>()
                {
                    expectedNode1
                }
            };
            var treeShape2 = new ActorBehaviorTreeShape
            {
                NodeList = new List<BehaviorNode>()
                {
                    expectedNode2
                }
            };

            // When
            var repository = new InMemoryActorBehaviorTreeRepository();

            // Validate Expected Initial State
            var initialState = repository.FindTreeShape(
                treeId
            );
            Assert.Empty(
                initialState.NodeList
            );
            repository.RegisterTree(
                treeId,
                treeShape1
            );
            repository.RegisterActorToTree(
                actorId1,
                treeId
            );
            repository.RegisterTree(
                treeId,
                treeShape2
            );
            repository.RegisterActorToTree(
                actorId2,
                treeId
            );

            var actual = repository.ActorIdList(
                treeId
            ).OrderBy(
                actorId => actorId
            );

            // Then
            Assert.Collection(
                actual,
                actorId => Assert.Equal(
                    expectedActorId1,
                    actorId
                ),
                actorId => Assert.Equal(
                    expectedActorId2,
                    actorId
                )
            );
        }
        [Fact]
        public void ShouldThrowArgumentExceptionWhenTreeIdIsNotRegistered()
        {
            // Given
            var treeId = "tree-id";
            var actorId = 1L;
            var expectedMessage = $"TreeId not found{Environment.NewLine}Parameter name: treeId";
            var expectedParam = "treeId";

            // When
            var repository = new InMemoryActorBehaviorTreeRepository();
            Action testAction = () => repository.UnRegisterActorFromTree(
                actorId,
                treeId
            );

            var actual = Record.Exception(
                testAction
            );

            // Then
            Assert.NotNull(
                actual
            );
            Assert.IsType<ArgumentException>(
                actual
            );
            Assert.Equal(
                expectedMessage,
                actual.Message
            );
            Assert.Equal(
                expectedParam,
                (actual as ArgumentException).ParamName
            );
        }
        [Fact]
        public void ShouldThrowExceptionOfArgumentExceptionWhenTreeIdIsNotFound()
        {
            // Given
            var treeId = "tree-id";
            var actorId = 1L;
            var expectedMessage = $"TreeId not found{Environment.NewLine}Parameter name: treeId";
            var expectedParam = "treeId";

            // When
            var repository = new InMemoryActorBehaviorTreeRepository();
            Action testAction = () => repository.RegisterActorToTree(
                actorId,
                treeId
            );

            var actual = Record.Exception(
                testAction
            );

            // Then
            Assert.NotNull(
                actual
            );
            Assert.IsType<ArgumentException>(
                actual
            );
            Assert.Equal(
                expectedMessage,
                actual.Message
            );
            Assert.Equal(
                expectedParam,
                (actual as ArgumentException).ParamName
            );
        }
        [Fact]
        public void ShouldReturnRegisteredTreeShapeAfterRegistered()
        {
            // Given
            var treeId = "tree-id";
            var treeShape = new ActorBehaviorTreeShape();
            var expected = treeShape;

            // When
            var repository = new InMemoryActorBehaviorTreeRepository();
            repository.RegisterTree(
                treeId,
                treeShape
            );
            var actual = repository.FindTreeShape(
                treeId
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
        [Fact]
        public void ShouldReturnEnumerableOfTreeIds()
        {
            // Given
            var treeId = "tree-id";
            var treeShape = new ActorBehaviorTreeShape();
            var expected = treeId;

            // When
            var repository = new InMemoryActorBehaviorTreeRepository();
            repository.RegisterTree(
                treeId,
                treeShape
            );
            var actual = repository.TreeIdList();

            // Then
            Assert.Collection(
                actual,
                actualTreeId => Assert.Equal(
                    expected, 
                    actualTreeId
                )
            );
        }
        [Fact]
        public void ShouldReturnDefaultTreeShapeWhenNotFound()
        {
            // Given
            var treeId = "tree-id";

            // When
            var repository = new InMemoryActorBehaviorTreeRepository();
            var actual = repository.FindTreeShape(
                treeId
            );

            // Then
            Assert.Empty(
                actual.NodeList
            );
        }
    }
}