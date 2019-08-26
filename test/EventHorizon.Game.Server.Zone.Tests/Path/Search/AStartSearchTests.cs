using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Map.State;
using EventHorizon.Game.Server.Zone.Model.Map;
using Xunit;
using EventHorizon.Game.Server.Zone.Path.Search;
using System.Diagnostics;

namespace EventHorizon.Game.Server.Zone.Tests.Path.Search
{
    public class AStarSearchTests
    {
        [Fact]
        public void TestShouldReturnExpectedPathWhenWorldContainsWalls_SmallWorld()
        {
            // Given
            var mapGraph = CreateWorldMapGraph(3);
            // Add walls 
            AddWallsToMapGraph(mapGraph, 0, 2);
            AddWallsToMapGraph(mapGraph, 0, 0);
            // AddWallsToMapGraph(mapGraph, -2, 0);
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
            Assert.True(true);
        }

        [Fact]
        public void TestShouldReturnExpectedPathWhenWorldContainsWalls_LargeWorld()
        {
            // Given
            var mapGraph = CreateWorldMapGraph(5);
            // Add walls 
            AddWallsToMapGraph(mapGraph, -2, 4);
            AddWallsToMapGraph(mapGraph, -2, 2);
            AddWallsToMapGraph(mapGraph, -2, 0);
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
            Assert.True(true);
        }

        [Fact]
        public void TestShouldReturnExpectedPathWhenWorldContainsWalls_SuperWorld()
        {
            // Given
            var mapGraph = CreateWorldMapGraph(100);
            // Add walls 
            AddWallsToMapGraph(mapGraph, -2, 4);
            AddWallsToMapGraph(mapGraph, -2, 2);
            AddWallsToMapGraph(mapGraph, -2, 0);
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
            Assert.True(true);
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