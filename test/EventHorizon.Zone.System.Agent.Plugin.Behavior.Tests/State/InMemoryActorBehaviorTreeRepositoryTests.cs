namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.State
{
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

    using global::System.Collections.Generic;

    using Xunit;

    public class InMemoryActorBehaviorTreeRepositoryTests
    {
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
            Assert.Null(
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
            Assert.False(
                actual.IsValid
            );
            Assert.Null(
                actual.NodeList
            );
        }

        [Fact]
        public void ShouldReplaceExistingShapeWhenShapeIsAddedWithSameIdOfExisting()
        {
            // Given
            var treeId = "tree-id";
            var treeShape = new ActorBehaviorTreeShape(
                treeId,
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode
                    {
                        Type = "CONDITION"
                    }
                }
            );
            var expected = new ActorBehaviorTreeShape(
                treeId,
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode
                    {
                        Type = "ACTION"
                    }
                }
            );

            // When
            var repository = new InMemoryActorBehaviorTreeRepository();
            repository.RegisterTree(
                treeId,
                treeShape
            );
            repository.RegisterTree(
                treeId,
                expected
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
    }
}
