namespace EventHorizon.Game.Server.Zone.Tests.Math
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Numerics;

    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Model.Map;

    using Xunit;
    using Xunit.Abstractions;

    public class TestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new()
        {
            new object[] { new Cell_ScenarioTestData { ExpectedIndex = 480, Input = new Vector3(-1, 0, 0), Expected = new Vector3(0, 0, 0)} },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public struct Cell_ScenarioTestData
    {
        public int ExpectedIndex { get; set; }
        public Vector3 Input { get; set; }
        public Vector3 Expected { get; set; }
    }
    public class Cell_ScenarioTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Cell_ScenarioTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        [Theory()]
        [ClassData(typeof(TestDataGenerator))]
        public void TestTheory(Cell_ScenarioTestData testData)
        {
            // var input = new Vector3(-1, 0, 0);
            var mapGraph = CreateTestingMap();
            var actual = mapGraph.GetClosestNode(testData.Input);

            Assert.Equal(testData.ExpectedIndex, actual.Index);
            Assert.Equal(testData.Expected, actual.Position);
        }

        private MapGraph CreateTestingMap()
        {
            var dim = 31;
            var tileDim = 4;
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
            Stopwatch stopWatch = new();

            for (var i = 0; i < zoneHeight; i++)
            {
                stopWatch.Restart();
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
                _testOutputHelper.WriteLine("Node {0} : {1}", i, stopWatch.ElapsedMilliseconds);
            }

            var currentNodeIndex = 0;
            // Setup edges for graph
            for (var i = 0; i < zoneHeight; i++)
            {
                stopWatch.Restart();
                for (var j = 0; j < zoneWidth; j++)
                {
                    var navNodeIndex = indexMap[currentNodeIndex];
                    // Top
                    var topIndex = GetTopIndex(i, j, zoneHeight);
                    if (topIndex > -1)
                    {
                        mapGraph.AddEdge(new MapEdge
                        {
                            FromIndex = navNodeIndex,
                            ToIndex = indexMap[topIndex]
                        });
                    }
                    // Right
                    var rightIndex = GetRightIndex(i, j, zoneWidth);
                    if (rightIndex > -1)
                    {
                        mapGraph.AddEdge(new MapEdge
                        {
                            FromIndex = navNodeIndex,
                            ToIndex = indexMap[rightIndex]
                        });
                    }
                    // Bottom
                    var bottomIndex = GetBottomIndex(i, j, zoneHeight);
                    if (bottomIndex > -1)
                    {
                        mapGraph.AddEdge(new MapEdge
                        {
                            FromIndex = navNodeIndex,
                            ToIndex = indexMap[bottomIndex]
                        });
                    }
                    // Left
                    var leftIndex = GetLeftIndex(i, j, zoneWidth);
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
                _testOutputHelper.WriteLine("Edge {0} : {1}", i, stopWatch.ElapsedMilliseconds);
            }
            return mapGraph;
        }

        private static int GetTopIndex(int cHeight, int cWidth, float maxHeight)
        {
            var index = cHeight - 1;
            if (index == -1)
            {
                return -1;
            }
            return (int)(index * maxHeight) + cWidth;
        }

        private static int GetRightIndex(int cHeight, int cWidth, float maxWidth)
        {
            var index = cWidth + 1;
            if (index == maxWidth)
            {
                return -1;
            }
            return (int)(cHeight * maxWidth) + index;
        }

        private static int GetLeftIndex(int cHeight, int cWidth, float maxWidth)
        {
            var index = cWidth - 1;
            if (index == -1)
            {
                return -1;
            }
            return (int)(cHeight * maxWidth) + index;
        }

        private static int GetBottomIndex(int cHeight, int cWidth, float maxHeight)
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
