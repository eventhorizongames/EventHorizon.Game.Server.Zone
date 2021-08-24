namespace EventHorizon.Zone.Core.Map.Generate
{
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Map.Generate;
    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Model.Map;

    using MediatR;

    public class GenerateMapFromDetailsHandler
        : IRequestHandler<GenerateMapFromDetails, IMapGraph>
    {
        public Task<IMapGraph> Handle(
            GenerateMapFromDetails request,
            CancellationToken cancellationToken
        )
        {
            var dim = request.MapDetails.Dimensions;
            var tileDim = request.MapDetails.TileDimensions;

            var dimensions = new Vector2(dim, dim);
            var tileDimension = tileDim;
            var zoneWidth = dimensions.X;
            var zoneHeight = dimensions.Y;

            var mapGraph = new MapGraph(
                new Vector3(
                    -(dim * tileDim / 2),
                    0,
                    -(dim * tileDim / 2)
                ),
                new Vector3(
                    dim * tileDim,
                    dim * tileDim,
                    dim * tileDim
                ),
                true
            );
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
            }

            return mapGraph.FromResult<IMapGraph>();
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
