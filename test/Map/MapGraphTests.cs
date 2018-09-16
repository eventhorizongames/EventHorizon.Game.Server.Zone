using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Map.Model;
using EventHorizon.Game.Server.Zone.Map.State;

namespace EventHorizon.Game.Server.Zone.Tests.Map
{
    public class MapGraphTests
    {
        [Fact]
        public void TestNumberOfNodes_ShouldReturnCountOfNodesAddedToMap()
        {
            // Given
            var inputPostion = Vector3.Zero;
            var inputDimensions = new Vector3(10, 10, 10);
            var inputIsDirectionGraph = true;

            var mapNode1 = new MapNode(Vector3.Zero);
            var mapNode2 = new MapNode(new Vector3(1));

            var expectedCount = 2;

            // When
            var mapGraph = new MapGraph(inputPostion, inputDimensions, inputIsDirectionGraph);
            mapGraph.AddNode(mapNode1);
            mapGraph.AddNode(mapNode2);

            var actual = mapGraph.NumberOfNodes;

            // Then
            Assert.Equal(expectedCount, actual);
        }

        [Fact]
        public void TestAll_ShouldReturnListOfAllAddedNodes()
        {
            // Given
            var inputPostion = Vector3.Zero;
            var inputDimensions = new Vector3(10, 10, 10);
            var inputIsDirectionGraph = true;

            var expectedMapNode1 = new MapNode(Vector3.Zero);
            var expectedMapNode2 = new MapNode(new Vector3(1));

            // When
            var mapGraph = new MapGraph(inputPostion, inputDimensions, inputIsDirectionGraph);
            expectedMapNode1 = mapGraph.AddNode(expectedMapNode1);
            expectedMapNode2 = mapGraph.AddNode(expectedMapNode2);

            var actual = mapGraph.All();

            // Then
            Assert.Collection(actual,
                a => Assert.Equal(expectedMapNode1, a),
                a => Assert.Equal(expectedMapNode2, a)
            );
        }

        [Fact]
        public void TestGetNode_ShouldReturnNodeWithFilledIndexGetNodeFromMap()
        {
            // Given
            var inputPostion = Vector3.Zero;
            var inputDimensions = new Vector3(10, 10, 10);
            var inputIsDirectionGraph = true;

            var expectedMapNode1 = new MapNode(Vector3.Zero);

            // When
            var mapGraph = new MapGraph(inputPostion, inputDimensions, inputIsDirectionGraph);
            expectedMapNode1 = mapGraph.AddNode(expectedMapNode1);

            var actual = mapGraph.GetNode(expectedMapNode1.Index);

            // Then
            Assert.Equal(expectedMapNode1, actual);
        }

        [Fact]
        public void TestGetClosestNodes_ShouldReturnNodeListWithinRadius()
        {
            // Given
            var mapPostion = Vector3.Zero;
            var mapDimensions = new Vector3(10, 10, 10);
            var mapIsDirectionGraph = true;
            var mapNode1 = new MapNode(new Vector3(0, 0, 0));
            var mapNode2 = new MapNode(new Vector3(1, 0, 0));
            var mapNode3 = new MapNode(new Vector3(1, 1, 0));
            var mapNode4 = new MapNode(new Vector3(1, 1, 1));
            var mapNode5 = new MapNode(new Vector3(2, 0, 0));
            var mapNode6 = new MapNode(new Vector3(2, 1, 0));
            var mapNode7 = new MapNode(new Vector3(2, 1, 1));

            var inputPosition = Vector3.Zero;
            var inputRadius = 1;

            var expectedPosition = Vector3.Zero;

            // When
            var mapGraph = new MapGraph(mapPostion, mapDimensions, mapIsDirectionGraph);
            var expectedMapNode1 = mapGraph.AddNode(mapNode1);
            var expectedMapNode2 = mapGraph.AddNode(mapNode2);
            var expectedMapNode3 = mapGraph.AddNode(mapNode3);
            var expectedMapNode4 = mapGraph.AddNode(mapNode4);
            var expectedMapNode5 = mapGraph.AddNode(mapNode5);
            var expectedMapNode6 = mapGraph.AddNode(mapNode6);
            var expectedMapNode7 = mapGraph.AddNode(mapNode7);

            var actual = mapGraph.GetClosestNodes(inputPosition, inputRadius);

            // Then
            Assert.Collection(actual,
                a => Assert.Equal(expectedMapNode1, a),
                a => Assert.Equal(expectedMapNode2, a)
            );
        }

        [Fact]
        public void TestGetClosestNode_ShouldReturnNodeAtPosition()
        {
            // Given
            var mapPostion = Vector3.Zero;
            var mapDimensions = new Vector3(10, 10, 10);
            var mapIsDirectionGraph = true;
            var mapNode1 = new MapNode(new Vector3(0, 0, 0));
            var mapNode2 = new MapNode(new Vector3(1, 0, 0));
            var mapNode3 = new MapNode(new Vector3(1, 1, 0));
            var mapNode4 = new MapNode(new Vector3(1, 1, 1));
            var mapNode5 = new MapNode(new Vector3(2, 0, 0));
            var mapNode6 = new MapNode(new Vector3(2, 1, 0));
            var mapNode7 = new MapNode(new Vector3(2, 1, 1));

            var inputPosition1 = Vector3.Zero;
            var inputPosition2 = new Vector3(1, 0, 0);
            var inputPosition3 = new Vector3(1.1f, 0, 0);

            // When
            var mapGraph = new MapGraph(mapPostion, mapDimensions, mapIsDirectionGraph);
            var expectedMapNode1 = mapGraph.AddNode(mapNode1);
            var expectedMapNode2 = mapGraph.AddNode(mapNode2);
            mapGraph.AddNode(mapNode3);
            mapGraph.AddNode(mapNode4);
            mapGraph.AddNode(mapNode5);
            mapGraph.AddNode(mapNode6);
            mapGraph.AddNode(mapNode7);

            var actual1 = mapGraph.GetClosestNode(inputPosition1);
            var actual2 = mapGraph.GetClosestNode(inputPosition2);
            var actual3 = mapGraph.GetClosestNode(inputPosition3);

            // Then
            Assert.Equal(expectedMapNode1, actual1);
            Assert.Equal(expectedMapNode2, actual2);
            Assert.Equal(expectedMapNode2, actual3); // Should find MapNode 2 based in input position 3
        }

        [Fact]
        public void TestGetEdge_ShouldReturnEdgeContainedInMap()
        {
            // Given
            var mapPostion = Vector3.Zero;
            var mapDimensions = new Vector3(10, 10, 10);
            var mapIsDirectionGraph = true;

            var mapNodeFrom = new MapNode(new Vector3(0, 0, 0));
            var mapNodeTo = new MapNode(new Vector3(1, 0, 0));

            // When
            var mapGraph = new MapGraph(mapPostion, mapDimensions, mapIsDirectionGraph);
            mapNodeFrom = mapGraph.AddNode(mapNodeFrom);
            mapNodeTo = mapGraph.AddNode(mapNodeTo);

            var expectedEdge = new MapEdge(mapNodeFrom.Index, mapNodeTo.Index);

            mapGraph.AddEdge(expectedEdge);

            var actual = mapGraph.GetEdge(mapNodeFrom.Index, mapNodeTo.Index);

            // Then
            Assert.Equal(expectedEdge, actual);
        }

        [Fact]
        public void TestGetEdge_ShouldReturnDefaultEdgeWhenNotInMap()
        {
            // Given
            var inputFrom = 1;
            var inputTo = 9999;
            var mapPostion = Vector3.Zero;
            var mapDimensions = new Vector3(10, 10, 10);
            var mapIsDirectionGraph = true;

            var expectedEdge = default(MapEdge);

            var mapNodeFrom = new MapNode(new Vector3(0, 0, 0));
            var mapNodeTo = new MapNode(new Vector3(1, 0, 0));

            // When
            var mapGraph = new MapGraph(mapPostion, mapDimensions, mapIsDirectionGraph);
            mapNodeFrom = mapGraph.AddNode(mapNodeFrom);
            mapNodeTo = mapGraph.AddNode(mapNodeTo);

            var mapEdge = new MapEdge(mapNodeFrom.Index, mapNodeTo.Index);

            mapGraph.AddEdge(mapEdge);

            var actual = mapGraph.GetEdge(inputFrom, inputTo);

            // Then
            Assert.Equal(expectedEdge, actual);
        }

        [Fact]
        public void TestAddNode_ShouldNotAddNodeIfNodeIsAddedTwiceWithSameId()
        {
            // Given
            var mapPostion = Vector3.Zero;
            var mapDimensions = new Vector3(10, 10, 10);
            var mapIsDirectionGraph = true;

            var expectedNode = new MapNode(new Vector3(0, 0, 0));

            // When
            var mapGraph = new MapGraph(mapPostion, mapDimensions, mapIsDirectionGraph);
            expectedNode = mapGraph.AddNode(expectedNode);

            var actual = mapGraph.AddNode(expectedNode);

            // Then
            Assert.Equal(expectedNode, actual);
            Assert.Collection(mapGraph.All(),
                a => Assert.Equal(expectedNode, actual)
            );
        }

        [Fact]
        public void TestAddEdge_ShouldOnlyAddEdgeIfBothFromAndToIndexAreNodesInMap()
        {
            // Given
            var inputFromIndex = 1;
            var inputToIndex = 9999;
            var mapPostion = Vector3.Zero;
            var mapDimensions = new Vector3(10, 10, 10);
            var mapIsDirectionGraph = true;

            var expectedEdge = default(MapEdge);

            // When
            var mapGraph = new MapGraph(mapPostion, mapDimensions, mapIsDirectionGraph);

            var mapEdge = new MapEdge(inputFromIndex, inputToIndex);

            mapGraph.AddEdge(mapEdge);

            var actual = mapGraph.GetEdge(inputFromIndex, inputToIndex);

            // Then
            Assert.Equal(expectedEdge, actual);
        }

        [Fact]
        public void TestAddEdge_WhenMapGraphIsNotDirectionalShouldAddReverseEdge()
        {
            // Given
            var mapPostion = Vector3.Zero;
            var mapDimensions = new Vector3(10, 10, 10);
            var mapIsDirectionGraph = false;

            var mapNodeFrom = new MapNode(new Vector3(0, 0, 0));
            var mapNodeTo = new MapNode(new Vector3(1, 0, 0));

            // When
            var mapGraph = new MapGraph(mapPostion, mapDimensions, mapIsDirectionGraph);
            mapNodeFrom = mapGraph.AddNode(mapNodeFrom);
            mapNodeTo = mapGraph.AddNode(mapNodeTo);

            var expectedEdge = new MapEdge(mapNodeFrom.Index, mapNodeTo.Index);
            var expectedReverseEdge = new MapEdge(mapNodeTo.Index, mapNodeFrom.Index);

            mapGraph.AddEdge(expectedEdge);

            var actual = mapGraph.GetEdge(mapNodeFrom.Index, mapNodeTo.Index);
            var actualReverse = mapGraph.GetEdge(mapNodeTo.Index, mapNodeFrom.Index);

            // Then
            Assert.Equal(expectedEdge, actual);
            Assert.Equal(expectedReverseEdge, actualReverse);
        }

        [Fact]
        public void TestGetEdgesOfNode_WhenNodeIndexMatchesFromShouldReturnListOfEdgesMatching()
        {
            // Given
            var mapPostion = Vector3.Zero;
            var mapDimensions = new Vector3(10, 10, 10);
            var mapIsDirectionGraph = false;

            var mapNodeFrom = new MapNode(new Vector3(0, 0, 0));
            var mapNodeTo = new MapNode(new Vector3(1, 0, 0));

            // When
            var mapGraph = new MapGraph(mapPostion, mapDimensions, mapIsDirectionGraph);
            mapNodeFrom = mapGraph.AddNode(mapNodeFrom);
            mapNodeTo = mapGraph.AddNode(mapNodeTo);

            var expectedEdge = new MapEdge(mapNodeFrom.Index, mapNodeTo.Index);

            mapGraph.AddEdge(expectedEdge);

            var actual = mapGraph.GetEdgesOfNode(mapNodeFrom.Index);

            // Then
            Assert.Collection(actual,
                a => Assert.Equal(expectedEdge, a)
            );
        }

        [Fact]
        public void TestGetEdgesOfNode_ShouldReturnEmptyListWhenNoNodesAreFound()
        {
            // Given
            var mapPostion = Vector3.Zero;
            var mapDimensions = new Vector3(10, 10, 10);
            var mapIsDirectionGraph = false;

            var inputNodeIndex = 999;

            // When
            var mapGraph = new MapGraph(mapPostion, mapDimensions, mapIsDirectionGraph);
            var actual = mapGraph.GetEdgesOfNode(inputNodeIndex);

            // Then
            Assert.Empty(actual);
        }
    }
}