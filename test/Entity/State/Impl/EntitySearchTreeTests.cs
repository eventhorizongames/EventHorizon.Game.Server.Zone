using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State.Impl;
using System.Numerics;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.State.Impl
{
    public class EntitySearchTreeTests
    {
        [Fact]
        public void TestUpdate_ShouldAddExpectedSearchEntitiesToSearchTree()
        {
            // Given
            var inputSearchEntity1 = new SearchEntity(1, Vector3.Zero);
            var inputSearchEntity2 = new SearchEntity(2, Vector3.Zero);

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Update(inputSearchEntity1);
            entitySearchTree.Update(inputSearchEntity2);

            // Then
            Assert.True(EntitySearchTree.SEARCH_OCTREE.Has(inputSearchEntity1));
            Assert.True(EntitySearchTree.SEARCH_OCTREE.Has(inputSearchEntity2));
        }
        [Fact]
        public void TestUpdate_ShouldUpdateEntityWhenCalledWithAMatchingId()
        {
            // Given
            var expectedEntity1Position = new Vector3(321);
            var expectedEntity2Position = new Vector3(321);

            var inputSearchEntity1 = new SearchEntity(1, expectedEntity1Position);
            var inputSearchEntity2 = new SearchEntity(2, Vector3.Zero);

            var inputAnotherSearchEntity2 = new SearchEntity(2, expectedEntity2Position);

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Update(inputSearchEntity1);
            entitySearchTree.Update(inputSearchEntity2);
            entitySearchTree.Update(inputAnotherSearchEntity2);

            // Then
            Assert.Collection(EntitySearchTree.SEARCH_OCTREE.All(),
                a => Assert.Equal(expectedEntity1Position, a.Position),
                a => Assert.Equal(expectedEntity2Position, a.Position)
            );
        }
        [Fact]
        public void TestUpdateDimensions_ShouldRebuildSearchTreeWithNewDimensions()
        {
            // Given
            var inputDimensions = new Vector3(100);
            var priorSearchOctree = EntitySearchTree.SEARCH_OCTREE;
            var inputSearchEntity1 = new SearchEntity(1, Vector3.Zero);
            var inputSearchEntity2 = new SearchEntity(2, Vector3.Zero);

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Update(inputSearchEntity1);
            entitySearchTree.Update(inputSearchEntity2);
            entitySearchTree.UpdateDimensions(inputDimensions);

            // Then
            Assert.True(EntitySearchTree.SEARCH_OCTREE.Has(inputSearchEntity1));
            Assert.True(EntitySearchTree.SEARCH_OCTREE.Has(inputSearchEntity2));
            Assert.NotEqual(priorSearchOctree, EntitySearchTree.SEARCH_OCTREE);
        }
    }
}