
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Load.Map;
using EventHorizon.Game.Server.Zone.Load.Map.Model;
using EventHorizon.Game.Server.Zone.Map;
using EventHorizon.Game.Server.Zone.Map.State;
using EventHorizon.Game.Server.Zone.Model.Map;
using EventHorizon.Game.Server.Zone.State;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Map.Create.Handler
{
    public class CreateMapHandler : INotificationHandler<CreateMapEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly ZoneMap _zoneMap;
        readonly IServerState _serverState;
        public CreateMapHandler(ILogger<CreateMapHandler> logger,
            IMediator mediator,
            IZoneMapFactory zoneMapFactory,
            IServerState serverState)
        {
            _logger = logger;
            _mediator = mediator;
            _zoneMap = zoneMapFactory.Map;
            _serverState = serverState;
        }

        public async Task Handle(CreateMapEvent notification, CancellationToken cancellationToken)
        {
            var dim = _zoneMap.Dimensions;
            var tileDim = _zoneMap.TileDimensions;
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
            Stopwatch stopWatch = new Stopwatch();

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
                _logger.LogInformation("Node {I} : {ElapsedMilliseconds}", i, stopWatch.ElapsedMilliseconds);
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
                _logger.LogInformation("Edge {Index} : {ElapsedMilliseconds}", i, stopWatch.ElapsedMilliseconds);
            }
            await _serverState.SetMap(mapGraph);
            await _mediator.Publish(new MapCreatedEvent());
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