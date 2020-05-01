namespace EventHorizon.Zone.Core.Map.Tests.Search
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Map.Search;
    using EventHorizon.Zone.Core.Model.Map;
    using FluentAssertions;
    using Xunit;

    public class AStarSearchTests
    {
        [Fact]
        public void ShouldReturnExpectedPathWhenWorldContainsDenseNodes()
        {
            // Given
            var expectedPath = new List<Vector3>
            {
                new Vector3(-2, 0, 2),
                new Vector3(-2, 0, 0),
                new Vector3(-2, 0, -2),
                new Vector3(0, 0, -2),
                new Vector3(2, 0, -2),
                new Vector3(2, 0, 0),
                new Vector3(2, 0, 2),
            };
            var mapGraph = CreateWorldMapGraph(3);
            // Add walls 
            AddWallsToMapGraph(mapGraph, 0, 2);
            AddWallsToMapGraph(mapGraph, 0, 0);
            // Start Node
            var startNode = mapGraph.GetClosestNode(new Vector3(-4, 0, 4));
            var toNode = mapGraph.GetClosestNode(new Vector3(4, 0, 4));

            // When
            var path = AStarSearch.CreatePath(
                mapGraph,
                startNode,
                toNode
            );

            // Then
            path.Should()
                .BeEquivalentTo(expectedPath);
        }

        [Fact]
        public void ShouldAllowForSearchingWhenAStartSearchIsInstnaced()
        {
            // Given
            var expectedPath = new List<Vector3>
            {
                new Vector3(-2, 0, 2),
                new Vector3(-2, 0, 0),
                new Vector3(-2, 0, -2),
                new Vector3(0, 0, -2),
                new Vector3(2, 0, -2),
                new Vector3(2, 0, 0),
                new Vector3(2, 0, 2),
            };
            var mapGraph = CreateWorldMapGraph(3);
            // Add walls 
            AddWallsToMapGraph(mapGraph, 0, 2);
            AddWallsToMapGraph(mapGraph, 0, 0);
            // Start Node
            var startNode = mapGraph.GetClosestNode(new Vector3(-4, 0, 4));
            var toNode = mapGraph.GetClosestNode(new Vector3(4, 0, 4));

            // When
            var algorithm = new AStarSearch();
            var actual = algorithm.Search(
                mapGraph,
                startNode,
                toNode
            );

            // Then
            actual.Should()
                .BeEquivalentTo(expectedPath);
        }

        [Theory(DisplayName = "A* Small Graph Path Search Testing ")]
        [Repeat(10)]
        [Trait("Category", "Performance")]
        public void ShouldReturnExpectedPathWhenWorldContainsWalls_SmallGraph(
            int iterationNumber
        )
        {
            // Given
            var expectedPath = new List<Vector3>
            {
                new Vector3(-2, 0, 2),
                new Vector3(-2, 0, 0),
                new Vector3(-2, 0, -2),
                new Vector3(0, 0, -2),
                new Vector3(2, 0, -2),
                new Vector3(2, 0, 0),
                new Vector3(2, 0, 2),
            };
            var mapGraph = CreateWorldMapGraph(3);
            // Add walls 
            AddWallsToMapGraph(mapGraph, 0, 2);
            AddWallsToMapGraph(mapGraph, 0, 0);
            // Start Node
            var startNode = mapGraph.GetClosestNode(new Vector3(-4, 0, 4));
            var toNode = mapGraph.GetClosestNode(new Vector3(4, 0, 4));

            // When
            var path = AStarSearch.CreatePath(
                mapGraph,
                startNode,
                toNode
            );

            // Then
            path.Should()
                .BeEquivalentTo(
                    expectedPath,
                    $"Path should be expected path at iteration of {iterationNumber}"
                );
        }

        [Theory(DisplayName = "A* Large Graph Path Search Testing ")]
        [Repeat(10)]
        [Trait("Category", "Performance")]
        public void TestShouldReturnExpectedPathWhenWorldContainsWalls_LargeGraph(
            int iterationNumber
        )
        {
            // Given
            var expected = new List<Vector3>
            {
                new Vector3(-20, 0, -20),
                new Vector3(-18, 0, -20),
                new Vector3(-16, 0, -20),
                new Vector3(-14, 0, -20),
                new Vector3(-14, 0, -18),
                new Vector3(-14, 0, -16),
                new Vector3(-14, 0, -14),
                new Vector3(-12, 0, -14),
                new Vector3(-12, 0, -12),
                new Vector3(-12, 0, -10),
                new Vector3(-12, 0, -8),
                new Vector3(-12, 0, -6),
                new Vector3(-12, 0, -4),
                new Vector3(-10, 0, -4),
                new Vector3(-10, 0, -2),
                new Vector3(-10, 0, 0),
                new Vector3(-10, 0, 2),
                new Vector3(-8, 0, 2),
                new Vector3(-6, 0, 2),
                new Vector3(-4, 0, 2),
                new Vector3(-4, 0, 4),
                new Vector3(-4, 0, 6),
                new Vector3(-4, 0, 8),
                new Vector3(-4, 0, 10),
                new Vector3(-2, 0, 10),
                new Vector3(0, 0, 10),
                new Vector3(2, 0, 10),
                new Vector3(2, 0, 12),
                new Vector3(4, 0, 12),
                new Vector3(6, 0, 12),
                new Vector3(8, 0, 12),
                new Vector3(10, 0, 12),
                new Vector3(12, 0, 12),
                new Vector3(12, 0, 14),
                new Vector3(12, 0, 16),
                new Vector3(14, 0, 16),
                new Vector3(14, 0, 18),
                new Vector3(16, 0, 18),
                new Vector3(18, 0, 18),
                new Vector3(20, 0, 18),
                new Vector3(20, 0, 20),
            };
            var mapGraph = CreateWorldMapGraph(25);
            // Add walls 
            for (int i = 0; i < 10; i++)
            {
                AddWallsToMapGraph(mapGraph, -2, i);
            }
            // Start Node
            var startNode = mapGraph.GetClosestNode(new Vector3(-20, 0, -20));
            var toNode = mapGraph.GetClosestNode(new Vector3(20, 0, 20));

            // When
            var path = AStarSearch.CreatePath(
                mapGraph,
                startNode,
                toNode
            );

            // Then
            path.Should()
                .BeEquivalentTo(
                    expected,
                    $"Path should be expected path at iteration of {iterationNumber}"
                );
        }

        [Theory(DisplayName = "A* Super Graph Path Search Testing ")]
        [Repeat(10)]
        [Trait("Category", "Performance")]
        public void ShouldReturnLessThanElapsedTimeWhenWorldContainsWalls_SuperGraph(
            int iterationNumber
        )
        {
            // Given
            var expectedPathCount = 100;
            var expectedMaxElapsedTime = 100;
            var mapGraph = CreateWorldMapGraph(100);
            // Add walls 
            for (int i = 0; i < 50; i++)
            {
                AddWallsToMapGraph(mapGraph, -2, i);
            }
            // Start Node
            var startNode = mapGraph.GetClosestNode(new Vector3(-100, 0, -100));
            var toNode = mapGraph.GetClosestNode(new Vector3(100, 0, 100));

            // When
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var path = AStarSearch.CreatePath(
                mapGraph,
                startNode,
                toNode
            );
            stopwatch.Stop();

            // Then
            path.Count
                .Should().BeGreaterThan(
                    expectedPathCount,
                    $"Path have a count greater than {expectedPathCount} path, at iteration of {iterationNumber}"
                );
            stopwatch.ElapsedMilliseconds
                .Should().BeLessThan(
                    expectedMaxElapsedTime,
                    $"Path should be under ElapsedMilliseconds"
                );
        }

        [Fact]
        public void ShouldReturnEmptyQueueWhenToMapNodeIsNegativeIndex()
        {
            // Given
            var nodeIndex = -1;
            var mapGraph = CreateWorldMapGraph(100);
            var fromMapNode = new MapNode(nodeIndex);
            var toMapNode = new MapNode(100);

            // When
            var actual = AStarSearch.CreatePath(
                mapGraph,
                fromMapNode,
                toMapNode
            );

            // Then
            actual.Should().BeEmpty();
        }

        private void AddWallsToMapGraph(MapGraph mapGraph, float x, float z)
        {
            var node = mapGraph.GetClosestNode(new Vector3(x, 0, z));
            var edges = mapGraph.GetEdgesOfNode(node.Index).ToList();
            IList<MapEdge> updatedEdges = new List<MapEdge>();
            for (int i = 0; i < edges.Count(); i++)
            {
                var edge = edges[i];
                edge.Cost = float.PositiveInfinity;
                updatedEdges.Add(edge);
            }
            foreach (var edge in edges)
            {
                mapGraph.RemoveEdge(edge);
            }
            foreach (var edge in updatedEdges)
            {
                mapGraph.AddEdge(edge);
            }
            // Do reverse edge
            foreach (var edge in updatedEdges)
            {
                var reverseEdge = mapGraph.GetEdge(edge.ToIndex, edge.FromIndex);
                mapGraph.RemoveEdge(reverseEdge);
                reverseEdge.Cost = float.PositiveInfinity;
                mapGraph.AddEdge(reverseEdge);
            }
        }

        private MapGraph CreateWorldMapGraph(int dim)
        {
            var tileDim = 2;
            var dimensions = new Vector2(dim, dim);
            var tileDimension = tileDim;
            var zoneWidth = dimensions.X;
            var zoneHeight = dimensions.Y;

            var mapGraph = new MapGraph(
                new Vector3(-(dim * tileDim / 2), 0, -(dim * tileDim / 2)),
                new Vector3(dim * tileDim, dim * tileDim, dim * tileDim),
                true);
            var width = zoneWidth * tileDimension;
            var height = zoneHeight * tileDimension;

            // Create Graph Nodes
            var indexMap = new List<int>();
            var key = 0;

            for (var i = 0; i < zoneHeight; i++)
            {
                for (var j = 0; j < zoneWidth; j++)
                {
                    float xPos = (i * tileDimension) + (tileDimension / 2);
                    xPos = (width / 2) - (width - xPos);
                    float zPos = (j * tileDimension) + (tileDimension / 2);
                    zPos = (height / 2) - (height - zPos);
                    var position = new Vector3(xPos, 0, zPos);
                    // Add node to graph
                    var navNode = new MapNode(position);
                    navNode = mapGraph.AddNode(navNode);
                    indexMap.Add(navNode.Index);
                    key++;
                }
            }

            var currentNodeIndex = 0;
            // Setup edges for graph
            for (var i = 0; i < zoneHeight; i++)
            {
                for (var j = 0; j < zoneWidth; j++)
                {
                    var navNodeIndex = indexMap[currentNodeIndex];
                    // Top
                    var topIndex = this.GetTopIndex(i, j, zoneHeight);
                    if (topIndex > -1)
                    {
                        mapGraph.AddEdge(new MapEdge
                        {
                            FromIndex = navNodeIndex,
                            ToIndex = indexMap[topIndex]
                        });
                    }
                    // Right
                    var rightIndex = this.GetRightIndex(i, j, zoneWidth);
                    if (rightIndex > -1)
                    {
                        mapGraph.AddEdge(new MapEdge
                        {
                            FromIndex = navNodeIndex,
                            ToIndex = indexMap[rightIndex]
                        });
                    }
                    // Bottom
                    var bottomIndex = this.GetBottomIndex(i, j, zoneHeight);
                    if (bottomIndex > -1)
                    {
                        mapGraph.AddEdge(new MapEdge
                        {
                            FromIndex = navNodeIndex,
                            ToIndex = indexMap[bottomIndex]
                        });
                    }
                    // Left
                    var leftIndex = this.GetLeftIndex(i, j, zoneWidth);
                    if (leftIndex > -1)
                    {
                        mapGraph.AddEdge(new MapEdge
                        {
                            FromIndex = navNodeIndex,
                            ToIndex = indexMap[leftIndex]
                        });
                    }

                    ++currentNodeIndex;
                }
            }
            return mapGraph;
        }
        private int GetTopIndex(int cHeight, int cWidth, float maxHeight)
        {
            var index = cHeight - 1;
            if (index == -1)
            {
                return -1;
            }
            return (int)(index * maxHeight) + cWidth;
        }
        private int GetRightIndex(int cHeight, int cWidth, float maxWidth)
        {
            var index = cWidth + 1;
            if (index == maxWidth)
            {
                return -1;
            }
            return (int)(cHeight * maxWidth) + index;
        }
        private int GetLeftIndex(int cHeight, int cWidth, float maxWidth)
        {
            var index = cWidth - 1;
            if (index == -1)
            {
                return -1;
            }
            return (int)(cHeight * maxWidth) + index;
        }
        private int GetBottomIndex(int cHeight, int cWidth, float maxHeight)
        {
            var index = cHeight + 1;
            if (index == maxHeight)
            {
                return -1;
            }
            return (int)(index * maxHeight) + cWidth;
        }
    }
}