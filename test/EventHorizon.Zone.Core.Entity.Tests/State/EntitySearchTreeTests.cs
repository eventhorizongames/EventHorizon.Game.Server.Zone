namespace EventHorizon.Zone.Core.Entity.Tests.State;

using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Entity.State;

using Xunit;

public class EntitySearchTreeTests
{
    [Fact]
    public async Task TestShouldAddExpectedSearchEntitiesToSearchTree()
    {
        // Given
        var inputSearchEntity1 = new SearchEntity(
            1,
            Vector3.Zero,
            new List<string>()
        );
        var inputSearchEntity2 = new SearchEntity(
            2,
            Vector3.Zero,
            new List<string>()
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            inputSearchEntity1
        );
        entitySearchTree.Add(
            inputSearchEntity2
        );

        var actual = await entitySearchTree.SearchInArea(
            Vector3.Zero, 9999
        );

        // Then
        Assert.Collection(
            actual,
            entity => Assert.Equal(inputSearchEntity1, entity),
            entity => Assert.Equal(inputSearchEntity2, entity)
        );
    }

    [Fact]
    public async Task TestShouldAllowForSearchingBasedOnPosition()
    {
        // Given
        var inputSearchPositionCenter = new Vector3(3);
        var inputSearchDistance = 3;
        var expectedSearchEntity = new SearchEntity(
            1, new Vector3(3),
            new List<string>()
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            expectedSearchEntity
        );
        var actual = await entitySearchTree.SearchInArea(
            inputSearchPositionCenter,
            inputSearchDistance
        );

        // Then
        Assert.Collection(
            actual,
            entity => Assert.Equal(expectedSearchEntity, entity)
        );
    }

    [Fact]
    public async Task TestShouldReturnAllWithinSearchDistance()
    {
        // Given
        var inputSearchPositionCenter = Vector3.Zero;
        var inputSearchDistance = 3f;
        var expectedSearchEntity1 = new SearchEntity(
            1,
            new Vector3(1, 0, 0),
            new List<string>()
        );
        var expectedSearchEntity2 = new SearchEntity(
            2,
            new Vector3(2, 0, 0),
            new List<string>()
        );
        var expectedSearchEntity3 = new SearchEntity(
            3,
            new Vector3(3, 0, 0),
            new List<string>()
        );
        var expectedSearchEntity4 = new SearchEntity(
            4,
            new Vector3(4, 0, 0),
            new List<string>()
        );
        var expectedSearchEntity5 = new SearchEntity(
            5,
            new Vector3(5, 0, 0),
            new List<string>()
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            expectedSearchEntity1
        );
        entitySearchTree.Add(
            expectedSearchEntity2
        );
        entitySearchTree.Add(
            expectedSearchEntity3
        );
        entitySearchTree.Add(
            expectedSearchEntity4
        );
        entitySearchTree.Add(
            expectedSearchEntity5
        );
        var actual = await entitySearchTree.SearchInArea(
            inputSearchPositionCenter,
            inputSearchDistance
        );

        // Then
        Assert.Collection(
            actual,
            entity => Assert.Equal(expectedSearchEntity1, entity),
            entity => Assert.Equal(expectedSearchEntity2, entity),
            entity => Assert.Equal(expectedSearchEntity3, entity)
        );
    }

    [Fact]
    public async Task TestShouldReturnAllWithinSearchDistanceAndWithSpecifiedTags()
    {
        // Given
        var inputSearchPositionCenter = Vector3.Zero;
        var inputSearchDistance = 3f;
        var inputFarSearchDistance = 123f;
        var inputSearchTagEnemyList = new List<string>() { "enemy" };
        var inputSearchTagPlayerList = new List<string>() { "player" };
        var inputSearchTagEnemyAndPlayerList = new List<string>() { "enemy", "player" };
        var inputSearchEmptyList = new List<string>() { };
        var expectedSearchEntity1 = new SearchEntity(
            1,
            new Vector3(1, 0, 0),
            new List<string>() { "enemy" }
        );
        var expectedSearchEntity2 = new SearchEntity(
            2,
            new Vector3(2, 0, 0),
            new List<string>() { "enemy", "player" }
        );
        var expectedSearchEntity3 = new SearchEntity(
            3,
            new Vector3(3, 0, 0),
            new List<string>() { "player" }
        );
        var expectedSearchEntity4 = new SearchEntity(
            4,
            new Vector3(4, 0, 0),
            new List<string>() { "player" }
        );
        var inputSearchEntity5 = new SearchEntity(
            5,
            new Vector3(5, 0, 0),
            new List<string>() { "not-enemy" }
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            expectedSearchEntity1
        );
        entitySearchTree.Add(
            expectedSearchEntity2
        );
        entitySearchTree.Add(
            expectedSearchEntity3
        );
        entitySearchTree.Add(
            expectedSearchEntity4
        );
        entitySearchTree.Add(
            inputSearchEntity5
        );
        var actualEntityListByEnemy = await entitySearchTree.SearchInAreaWithTag(
            inputSearchPositionCenter,
            inputSearchDistance,
            inputSearchTagEnemyList
        );
        var actualEntityListByPlayer = await entitySearchTree.SearchInAreaWithTag(
            inputSearchPositionCenter,
            inputFarSearchDistance,
            inputSearchTagPlayerList
        );
        var actualEntityListByPlayerAndEnemy = await entitySearchTree.SearchInAreaWithTag(
            inputSearchPositionCenter,
            inputFarSearchDistance,
            inputSearchTagEnemyAndPlayerList
        );
        var actualEntityListEmpty = await entitySearchTree.SearchInAreaWithTag(
            inputSearchPositionCenter,
            inputFarSearchDistance,
            inputSearchEmptyList
        );

        // Then
        Assert.Collection(
            actualEntityListByEnemy,
            entity => Assert.Equal(expectedSearchEntity1, entity),
            entity => Assert.Equal(expectedSearchEntity2, entity)
        );
        Assert.Collection(
            actualEntityListByPlayer,
            entity => Assert.Equal(expectedSearchEntity2, entity),
            entity => Assert.Equal(expectedSearchEntity3, entity),
            entity => Assert.Equal(expectedSearchEntity4, entity)
        );
        Assert.Collection(
            actualEntityListByPlayerAndEnemy,
            entity => Assert.Equal(expectedSearchEntity1, entity),
            entity => Assert.Equal(expectedSearchEntity2, entity),
            entity => Assert.Equal(expectedSearchEntity3, entity),
            entity => Assert.Equal(expectedSearchEntity4, entity)
        );
        Assert.Empty(
            actualEntityListEmpty
        );
    }

    [Fact]
    public async Task TestShouldNotBeAffectedByANullEntityTagListWhenRequestingASearchForASingleTag()
    {
        // Given
        var inputSearchPositionCenter = Vector3.Zero;
        var inputSearchDistance = 3f;
        var inputSearchTagEnemyList = new List<string>() { "enemy" };
        var expectedSearchEntity1 = new SearchEntity(
            1,
            new Vector3(1, 0, 0),
            null
        );
        var expectedSearchEntity2 = new SearchEntity(
            2,
            new Vector3(2, 0, 0),
            new List<string>() { "enemy" }
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            expectedSearchEntity1
        );
        entitySearchTree.Add(
            expectedSearchEntity2
        );
        var actual = await entitySearchTree.SearchInAreaWithTag(
            inputSearchPositionCenter,
            inputSearchDistance,
            inputSearchTagEnemyList
        );

        // Then
        Assert.Collection(
            actual,
            entity => Assert.Equal(expectedSearchEntity2, entity)
        );
    }

    [Fact]
    public async Task TestShouldNotBeAffectedByANullPassedTagList()
    {
        // Given
        var inputSearchPositionCenter = Vector3.Zero;
        var inputSearchDistance = 3f;
        IList<string> inputSearchTagEnemyList = null;
        var expectedSearchEntity1 = new SearchEntity(
            1,
            new Vector3(1, 0, 0),
            new List<string>() { "enemy" }
        );
        var expectedSearchEntity2 = new SearchEntity(
            2,
            new Vector3(2, 0, 0),
            new List<string>() { "enemy" }
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            expectedSearchEntity1
        );
        entitySearchTree.Add(
            expectedSearchEntity2
        );
        var actual = await entitySearchTree.SearchInAreaWithTag(
            inputSearchPositionCenter,
            inputSearchDistance,
            inputSearchTagEnemyList
        );

        // Then
        Assert.Empty(
            actual
        );
    }

    [Fact]
    public async Task TestShouldReturnAllWithinSearchDistanceAndWithAllSpecifiedTags()
    {
        // Given
        var inputSearchPositionCenter = Vector3.Zero;
        var inputSearchDistance = 3f;
        var inputFarSearchDistance = 123f;
        var inputSearchTagEnemyList = new List<string>() { "enemy" };
        var inputSearchTagPlayerList = new List<string>() { "player" };
        var inputSearchTagEnemyAndPlayerList = new List<string>() { "enemy", "player" };
        var inputSearchEmptyList = new List<string>() { };
        var expectedSearchEntity1 = new SearchEntity(
            1,
            new Vector3(1, 0, 0),
            new List<string>() { "enemy" }
        );
        var expectedSearchEntity2 = new SearchEntity(
            2,
            new Vector3(2, 0, 0),
            new List<string>() { "enemy", "player" }
        );
        var expectedSearchEntity3 = new SearchEntity(
            3,
            new Vector3(3, 0, 0),
            new List<string>() { "player" }
        );
        var expectedSearchEntity4 = new SearchEntity(
            4,
            new Vector3(4, 0, 0),
            new List<string>() { "player" }
        );
        var inputSearchEntity5 = new SearchEntity(
            5,
            new Vector3(5, 0, 0),
            new List<string>() { "not-enemy" }
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            expectedSearchEntity1
        );
        entitySearchTree.Add(
            expectedSearchEntity2
        );
        entitySearchTree.Add(
            expectedSearchEntity3
        );
        entitySearchTree.Add(
            expectedSearchEntity4
        );
        entitySearchTree.Add(
            inputSearchEntity5
        );
        var actualEntityListByEnemy = await entitySearchTree.SearchInAreaWithAllTags(
            inputSearchPositionCenter,
            inputSearchDistance,
            inputSearchTagEnemyList
        );
        var actualEntityListByPlayer = await entitySearchTree.SearchInAreaWithAllTags(
            inputSearchPositionCenter,
            inputFarSearchDistance,
            inputSearchTagPlayerList
        );
        var actualEntityListByPlayerAndEnemy = await entitySearchTree.SearchInAreaWithAllTags(
            inputSearchPositionCenter,
            inputFarSearchDistance,
            inputSearchTagEnemyAndPlayerList
        );
        var actualEntityListEmpty = await entitySearchTree.SearchInAreaWithAllTags(
            inputSearchPositionCenter,
            inputFarSearchDistance,
            inputSearchEmptyList
        );

        // Then
        Assert.Collection(
            actualEntityListByEnemy,
            entity => Assert.Equal(expectedSearchEntity1, entity),
            entity => Assert.Equal(expectedSearchEntity2, entity)
        );
        Assert.Collection(
            actualEntityListByPlayer,
            entity => Assert.Equal(expectedSearchEntity2, entity),
            entity => Assert.Equal(expectedSearchEntity3, entity),
            entity => Assert.Equal(expectedSearchEntity4, entity)
        );
        Assert.Collection(
            actualEntityListByPlayerAndEnemy,
            entity => Assert.Equal(expectedSearchEntity2, entity)
        );
        Assert.Empty(
            actualEntityListEmpty
        );
    }

    [Fact]
    public async Task TestShouldNotBeAffectedByANullEntityTagListWhenRequestingASearchForAllTags()
    {
        // Given
        var inputSearchPositionCenter = Vector3.Zero;
        var inputSearchDistance = 3f;
        var inputSearchTagEnemyList = new List<string>() { "enemy" };
        var expectedSearchEntity1 = new SearchEntity(
            1,
            new Vector3(1, 0, 0),
            null
        );
        var expectedSearchEntity2 = new SearchEntity(
            2,
            new Vector3(2, 0, 0),
            new List<string>() { "enemy" }
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            expectedSearchEntity1
        );
        entitySearchTree.Add(
            expectedSearchEntity2
        );
        var actual = await entitySearchTree.SearchInAreaWithAllTags(
            inputSearchPositionCenter,
            inputSearchDistance,
            inputSearchTagEnemyList
        );

        // Then
        Assert.Collection(
            actual,
            entity => Assert.Equal(expectedSearchEntity2, entity)
        );
    }

    [Fact]
    public async Task TestShouldNotBeAffectedByANullPassedTagListWhenRequestingASearchForAllTags()
    {
        // Given
        var inputSearchPositionCenter = Vector3.Zero;
        var inputSearchDistance = 3f;
        IList<string> inputSearchTagEnemyList = null;
        var expectedSearchEntity1 = new SearchEntity(
            1,
            new Vector3(1, 0, 0),
            new List<string>() { "enemy" }
        );
        var expectedSearchEntity2 = new SearchEntity(
            2,
            new Vector3(2, 0, 0),
            new List<string>() { "enemy" }
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            expectedSearchEntity1
        );
        entitySearchTree.Add(
            expectedSearchEntity2
        );
        var actual = await entitySearchTree.SearchInAreaWithAllTags(
            inputSearchPositionCenter,
            inputSearchDistance,
            inputSearchTagEnemyList
        );

        // Then
        Assert.Empty(
            actual
        );
    }

    [Fact]
    public async Task TestShouldAddEntityOnlyOnceWhenCalledWithAMatchingId()
    {
        // Given
        var expectedEntity1Position = new Vector3(321);
        var expectedEntity2Position = new Vector3(321);

        var inputSearchEntity1 = new SearchEntity(
            1,
            expectedEntity1Position,
            new List<string>()
        );
        var inputSearchEntity2 = new SearchEntity(
            2,
            Vector3.Zero,
            new List<string>()
        );

        var inputAnotherSearchEntity2 = new SearchEntity(
            2,
            expectedEntity2Position,
            new List<string>()
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            inputSearchEntity1
        );
        entitySearchTree.Add(
            inputSearchEntity2
        );
        entitySearchTree.Add(
            inputAnotherSearchEntity2
        );

        var actual = await entitySearchTree.SearchInArea(
            new Vector3(0),
            9999
        );

        // Then
        Assert.Collection(
            actual,
            entity => Assert.Equal(expectedEntity1Position, entity.Position),
            entity => Assert.Equal(expectedEntity2Position, entity.Position)
        );
    }

    [Fact]
    public async Task TestShouldRebuildSearchTreeWithNewDimensionsKeepingTheEntityListIntact()
    {
        // Given
        var inputDimensions = new Vector3(100);
        var inputSearchEntity1 = new SearchEntity(
            1,
            Vector3.Zero,
            new List<string>()
        );
        var inputSearchEntity2 = new SearchEntity(
            2,
            Vector3.Zero,
            new List<string>()
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            inputSearchEntity1
        );
        entitySearchTree.Add(
            inputSearchEntity2
        );
        entitySearchTree.UpdateDimensions(
            inputDimensions
        );

        var actual = await entitySearchTree.SearchInArea(
            new Vector3(0),
            9999
        );

        // Then
        Assert.Collection(
            actual,
            entity => Assert.Equal(inputSearchEntity1, entity),
            entity => Assert.Equal(inputSearchEntity2, entity)
        );
    }

    [Fact]
    public async Task TestShouldRemoveEntityWhenRemoveIsCalled()
    {
        // Given
        var entity1Position = new Vector3(321);
        var entity2Position = new Vector3(0);

        var inputSearchEntity1 = new SearchEntity(
            1,
            entity1Position,
            new List<string>()
        );
        var inputSearchEntity2 = new SearchEntity(
            2,
            Vector3.Zero,
            new List<string>()
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            inputSearchEntity1
        );
        entitySearchTree.Add(
            inputSearchEntity2
        );

        var assertContains = await entitySearchTree.SearchInArea(
            new Vector3(0),
            9999
        );
        Assert.Collection(
            assertContains,
            entity => Assert.Equal(entity1Position, entity.Position),
            entity => Assert.Equal(entity2Position, entity.Position)
        );

        entitySearchTree.Remove(
            inputSearchEntity2
        );

        var actual = await entitySearchTree.SearchInArea(
            new Vector3(0),
            9999
        );
        // Then
        Assert.Collection(
            actual,
            entity => Assert.Equal(entity1Position, entity.Position)
        );
    }

    [Fact]
    public async Task TestShouldFindExpectedEntityListWhenWithinDimensionsFromCenterPosition()
    {
        // Given
        var inputSearchPositionCenter = Vector3.Zero;
        var inputSearchDimension = new Vector3(2);
        var inputSearchTagEnemyList = new List<string>() { "enemy" };
        var expectedSearchEntity1 = new SearchEntity(
            1,
            new Vector3(1, 0, 0),
            new List<string>() { "enemy" }
        );
        var expectedSearchEntity2 = new SearchEntity(
            2,
            new Vector3(3, 0, 0),
            new List<string>() { "enemy" }
        );

        // When
        var entitySearchTree = new InMemoryEntitySearchTree();
        entitySearchTree.Reset();
        entitySearchTree.Add(
            expectedSearchEntity1
        );
        entitySearchTree.Add(
            expectedSearchEntity2
        );
        var actual = await entitySearchTree.SearchInArea(
            inputSearchPositionCenter,
            inputSearchDimension
        );

        // Then
        Assert.Collection(
            actual,
            entity => Assert.Equal(expectedSearchEntity1, entity)
        );
    }
}
