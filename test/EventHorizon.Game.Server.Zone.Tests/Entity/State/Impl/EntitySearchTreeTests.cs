using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State.Impl;
using System.Numerics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.State.Impl
{
    public class EntitySearchTreeTests
    {
        [Fact]
        public async Task TestAdd_ShouldAddExpectedSearchEntitiesToSearchTree()
        {
            // Given
            var inputSearchEntity1 = new SearchEntity(1, Vector3.Zero, new List<string>());
            var inputSearchEntity2 = new SearchEntity(2, Vector3.Zero, new List<string>());

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Add(inputSearchEntity1);
            entitySearchTree.Add(inputSearchEntity2);

            var actual = await entitySearchTree.SearchInArea(Vector3.Zero, 9999);

            // Then
            Assert.Collection(actual,
                a => Assert.Equal(inputSearchEntity1, a),
                a => Assert.Equal(inputSearchEntity2, a)
            );
        }
        [Fact]
        public async Task TestSearchInArea_ShouldAllowForSearchingBasedOnPosition()
        {
            // Given
            var inputSearchPositionCenter = new Vector3(3);
            var inputSearchDistance = 3;
            var expectedSearchEntity = new SearchEntity(1, new Vector3(3), new List<string>());

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Add(expectedSearchEntity);
            var entityList = await entitySearchTree.SearchInArea(inputSearchPositionCenter, inputSearchDistance);

            // Then
            Assert.Collection(entityList,
                a => Assert.Equal(expectedSearchEntity, a)
            );
        }
        [Fact]
        public async Task TestSearchInArea_ShouldReturnAllWithinSearchDistance()
        {
            // Given
            var inputSearchPositionCenter = Vector3.Zero;
            var inputSearchDistance = 3f;
            var expectedSearchEntity1 = new SearchEntity(1, new Vector3(1, 0, 0), new List<string>());
            var expectedSearchEntity2 = new SearchEntity(2, new Vector3(2, 0, 0), new List<string>());
            var expectedSearchEntity3 = new SearchEntity(3, new Vector3(3, 0, 0), new List<string>());
            var expectedSearchEntity4 = new SearchEntity(4, new Vector3(4, 0, 0), new List<string>());
            var expectedSearchEntity5 = new SearchEntity(5, new Vector3(5, 0, 0), new List<string>());

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Add(expectedSearchEntity1);
            entitySearchTree.Add(expectedSearchEntity2);
            entitySearchTree.Add(expectedSearchEntity3);
            entitySearchTree.Add(expectedSearchEntity4);
            entitySearchTree.Add(expectedSearchEntity5);
            var entityList = await entitySearchTree.SearchInArea(inputSearchPositionCenter, inputSearchDistance);

            // Then
            Assert.Collection(entityList,
                a => Assert.Equal(expectedSearchEntity1, a),
                a => Assert.Equal(expectedSearchEntity2, a),
                a => Assert.Equal(expectedSearchEntity3, a)
            );
        }
        [Fact]
        public async Task TestSearchInAreaWithTag_ShouldReturnAllWithinSearchDistanceAndWithSpecifiedTags()
        {
            // Given
            var inputSearchPositionCenter = Vector3.Zero;
            var inputSearchDistance = 3f;
            var inputFarSearchDistance = 123f;
            var inputSearchTagEnemyList = new List<string>() { "enemy" };
            var inputSearchTagPlayerList = new List<string>() { "player" };
            var inputSearchTagEnemyAndPlayerList = new List<string>() { "enemy", "player" };
            var inputSearchEmptyList = new List<string>() { };
            var expectedSearchEntity1 = new SearchEntity(1, new Vector3(1, 0, 0), new List<string>() { "enemy" });
            var expectedSearchEntity2 = new SearchEntity(2, new Vector3(2, 0, 0), new List<string>() { "enemy", "player" });
            var expectedSearchEntity3 = new SearchEntity(3, new Vector3(3, 0, 0), new List<string>() { "player" });
            var expectedSearchEntity4 = new SearchEntity(4, new Vector3(4, 0, 0), new List<string>() { "player" });
            var inputSearchEntity5 = new SearchEntity(5, new Vector3(5, 0, 0), new List<string>() { "not-enemy" });

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Add(expectedSearchEntity1);
            entitySearchTree.Add(expectedSearchEntity2);
            entitySearchTree.Add(expectedSearchEntity3);
            entitySearchTree.Add(expectedSearchEntity4);
            entitySearchTree.Add(inputSearchEntity5);
            var entityListByEnemy = await entitySearchTree.SearchInAreaWithTag(inputSearchPositionCenter, inputSearchDistance, inputSearchTagEnemyList);
            var entityListByPlayer = await entitySearchTree.SearchInAreaWithTag(inputSearchPositionCenter, inputFarSearchDistance, inputSearchTagPlayerList);
            var entityListByPlayerAndEnemy = await entitySearchTree.SearchInAreaWithTag(inputSearchPositionCenter, inputFarSearchDistance, inputSearchTagEnemyAndPlayerList);
            var entityListEmpty = await entitySearchTree.SearchInAreaWithTag(inputSearchPositionCenter, inputFarSearchDistance, inputSearchEmptyList);

            // Then
            Assert.Collection(entityListByEnemy,
                a => Assert.Equal(expectedSearchEntity1, a),
                a => Assert.Equal(expectedSearchEntity2, a)
            );
            Assert.Collection(entityListByPlayer,
                a => Assert.Equal(expectedSearchEntity2, a),
                a => Assert.Equal(expectedSearchEntity3, a),
                a => Assert.Equal(expectedSearchEntity4, a)
            );
            Assert.Collection(entityListByPlayerAndEnemy,
                a => Assert.Equal(expectedSearchEntity1, a),
                a => Assert.Equal(expectedSearchEntity2, a),
                a => Assert.Equal(expectedSearchEntity3, a),
                a => Assert.Equal(expectedSearchEntity4, a)
            );
            Assert.Empty(entityListEmpty);
        }
        [Fact]
        public async Task TestSearchInAreaWithTag_ShouldNotBeAffectedByANullEntityTagList()
        {
            // Given
            var inputSearchPositionCenter = Vector3.Zero;
            var inputSearchDistance = 3f;
            var inputSearchTagEnemyList = new List<string>() { "enemy" };
            var expectedSearchEntity1 = new SearchEntity(1, new Vector3(1, 0, 0), null);
            var expectedSearchEntity2 = new SearchEntity(2, new Vector3(2, 0, 0), new List<string>() { "enemy" });

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Add(expectedSearchEntity1);
            entitySearchTree.Add(expectedSearchEntity2);
            var entityListByEnemy = await entitySearchTree.SearchInAreaWithTag(inputSearchPositionCenter, inputSearchDistance, inputSearchTagEnemyList);

            // Then
            Assert.Collection(entityListByEnemy,
                a => Assert.Equal(expectedSearchEntity2, a)
            );
        }
        [Fact]
        public async Task TestSearchInAreaWithTag_ShouldNotBeAffectedByANullPassedTagList()
        {
            // Given
            var inputSearchPositionCenter = Vector3.Zero;
            var inputSearchDistance = 3f;
            IList<string> inputSearchTagEnemyList = null;
            var expectedSearchEntity1 = new SearchEntity(1, new Vector3(1, 0, 0), new List<string>() { "enemy" });
            var expectedSearchEntity2 = new SearchEntity(2, new Vector3(2, 0, 0), new List<string>() { "enemy" });

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Add(expectedSearchEntity1);
            entitySearchTree.Add(expectedSearchEntity2);
            var entityListByEnemy = await entitySearchTree.SearchInAreaWithTag(inputSearchPositionCenter, inputSearchDistance, inputSearchTagEnemyList);

            // Then
            Assert.Empty(entityListByEnemy);
        }
        [Fact]
        public async Task TestSearchInAreaWithAllTags_ShouldReturnAllWithinSearchDistanceAndWithAllSpecifiedTags()
        {
            // Given
            var inputSearchPositionCenter = Vector3.Zero;
            var inputSearchDistance = 3f;
            var inputFarSearchDistance = 123f;
            var inputSearchTagEnemyList = new List<string>() { "enemy" };
            var inputSearchTagPlayerList = new List<string>() { "player" };
            var inputSearchTagEnemyAndPlayerList = new List<string>() { "enemy", "player" };
            var inputSearchEmptyList = new List<string>() { };
            var expectedSearchEntity1 = new SearchEntity(1, new Vector3(1, 0, 0), new List<string>() { "enemy" });
            var expectedSearchEntity2 = new SearchEntity(2, new Vector3(2, 0, 0), new List<string>() { "enemy", "player" });
            var expectedSearchEntity3 = new SearchEntity(3, new Vector3(3, 0, 0), new List<string>() { "player" });
            var expectedSearchEntity4 = new SearchEntity(4, new Vector3(4, 0, 0), new List<string>() { "player" });
            var inputSearchEntity5 = new SearchEntity(5, new Vector3(5, 0, 0), new List<string>() { "not-enemy" });

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Add(expectedSearchEntity1);
            entitySearchTree.Add(expectedSearchEntity2);
            entitySearchTree.Add(expectedSearchEntity3);
            entitySearchTree.Add(expectedSearchEntity4);
            entitySearchTree.Add(inputSearchEntity5);
            var entityListByEnemy = await entitySearchTree.SearchInAreaWithAllTags(inputSearchPositionCenter, inputSearchDistance, inputSearchTagEnemyList);
            var entityListByPlayer = await entitySearchTree.SearchInAreaWithAllTags(inputSearchPositionCenter, inputFarSearchDistance, inputSearchTagPlayerList);
            var entityListByPlayerAndEnemy = await entitySearchTree.SearchInAreaWithAllTags(inputSearchPositionCenter, inputFarSearchDistance, inputSearchTagEnemyAndPlayerList);
            var entityListEmpty = await entitySearchTree.SearchInAreaWithAllTags(inputSearchPositionCenter, inputFarSearchDistance, inputSearchEmptyList);

            // Then
            Assert.Collection(entityListByEnemy,
                a => Assert.Equal(expectedSearchEntity1, a),
                a => Assert.Equal(expectedSearchEntity2, a)
            );
            Assert.Collection(entityListByPlayer,
                a => Assert.Equal(expectedSearchEntity2, a),
                a => Assert.Equal(expectedSearchEntity3, a),
                a => Assert.Equal(expectedSearchEntity4, a)
            );
            Assert.Collection(entityListByPlayerAndEnemy,
                a => Assert.Equal(expectedSearchEntity2, a)
            );
            Assert.Empty(entityListEmpty);
        }
        [Fact]
        public async Task TestSearchInAreaWithAllTags_ShouldNotBeAffectedByANullEntityTagList()
        {
            // Given
            var inputSearchPositionCenter = Vector3.Zero;
            var inputSearchDistance = 3f;
            var inputSearchTagEnemyList = new List<string>() { "enemy" };
            var expectedSearchEntity1 = new SearchEntity(1, new Vector3(1, 0, 0), null);
            var expectedSearchEntity2 = new SearchEntity(2, new Vector3(2, 0, 0), new List<string>() { "enemy" });

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Add(expectedSearchEntity1);
            entitySearchTree.Add(expectedSearchEntity2);
            var entityListByEnemy = await entitySearchTree.SearchInAreaWithAllTags(inputSearchPositionCenter, inputSearchDistance, inputSearchTagEnemyList);

            // Then
            Assert.Collection(entityListByEnemy,
                a => Assert.Equal(expectedSearchEntity2, a)
            );
        }
        [Fact]
        public async Task TestSearchInAreaWithAllTags_ShouldNotBeAffectedByANullPassedTagList()
        {
            // Given
            var inputSearchPositionCenter = Vector3.Zero;
            var inputSearchDistance = 3f;
            IList<string> inputSearchTagEnemyList = null;
            var expectedSearchEntity1 = new SearchEntity(1, new Vector3(1, 0, 0), new List<string>() { "enemy" });
            var expectedSearchEntity2 = new SearchEntity(2, new Vector3(2, 0, 0), new List<string>() { "enemy" });

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Add(expectedSearchEntity1);
            entitySearchTree.Add(expectedSearchEntity2);
            var entityListByEnemy = await entitySearchTree.SearchInAreaWithAllTags(inputSearchPositionCenter, inputSearchDistance, inputSearchTagEnemyList);

            // Then
            Assert.Empty(entityListByEnemy);
        }
        [Fact]
        public async Task TestAdd_ShouldAddEntityWhenCalledWithAMatchingId()
        {
            // Given
            var expectedEntity1Position = new Vector3(321);
            var expectedEntity2Position = new Vector3(321);

            var inputSearchEntity1 = new SearchEntity(1, expectedEntity1Position, new List<string>());
            var inputSearchEntity2 = new SearchEntity(2, Vector3.Zero, new List<string>());

            var inputAnotherSearchEntity2 = new SearchEntity(2, expectedEntity2Position, new List<string>());

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Add(inputSearchEntity1);
            entitySearchTree.Add(inputSearchEntity2);
            entitySearchTree.Add(inputAnotherSearchEntity2);

            var actual = await entitySearchTree.SearchInArea(new Vector3(0), 9999);

            // Then
            Assert.Collection(actual,
                a => Assert.Equal(expectedEntity1Position, a.Position),
                a => Assert.Equal(expectedEntity2Position, a.Position)
            );
        }
        [Fact]
        public async Task TestUpdateDimensions_ShouldRebuildSearchTreeWithNewDimensions()
        {
            // Given
            var inputDimensions = new Vector3(100);
            var inputSearchEntity1 = new SearchEntity(1, Vector3.Zero, new List<string>());
            var inputSearchEntity2 = new SearchEntity(2, Vector3.Zero, new List<string>());

            // When
            var entitySearchTree = new EntitySearchTree();
            entitySearchTree.Add(inputSearchEntity1);
            entitySearchTree.Add(inputSearchEntity2);
            entitySearchTree.UpdateDimensions(inputDimensions);

            var actual = await entitySearchTree.SearchInArea(new Vector3(0), 9999);

            // Then
            Assert.Collection(actual,
                a => Assert.Equal(inputSearchEntity1, a),
                a => Assert.Equal(inputSearchEntity2, a)
            );
        }
    }
}