
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;
using EventHorizon.Zone.Core.Map.Model;
using EventHorizon.Zone.Core.Events.Map.Create;
using Microsoft.Extensions.Logging;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.Core.Map.State;
using EventHorizon.Performance;
using System.IO;
using EventHorizon.Zone.Core.Model.Info;

namespace EventHorizon.Zone.Core.Map.Create
{
    public struct CreateMapHandler : INotificationHandler<CreateMapEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;
        readonly IServerMap _serverMap;
        readonly IPerformanceTracker _performanceTracker;

        public CreateMapHandler(
            ILogger<CreateMapHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            IServerMap serverMap,
            IPerformanceTracker performanceTracker
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _serverMap = serverMap;
            _performanceTracker = performanceTracker;
        }

        public async Task Handle(CreateMapEvent notification, CancellationToken cancellationToken)
        {
            using (_performanceTracker.Track(
                "Create Map"
            ))
            {
                // Load in ZoneMapFile
                var zoneMap = await GetZoneMapDetails();

                var dim = zoneMap.Dimensions;
                var tileDim = zoneMap.TileDimensions;
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
                    // _logger.LogInformation("Node {I} : {ElapsedMilliseconds}ms", i, stopWatch.ElapsedMilliseconds);
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
                    // _logger.LogInformation("Edge {Index} : {ElapsedMilliseconds}ms", i, stopWatch.ElapsedMilliseconds);
                }
                _serverMap.SetMap(mapGraph);
                _serverMap.SetMapDetails(zoneMap);
                _serverMap.SetMapMesh(zoneMap.Mesh);
                await _mediator.Publish(new MapCreatedEvent());
            }
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


        private async Task<ZoneMapDetails> GetZoneMapDetails()
        {
            if (DoesZoneMapDetailsFileNotExist())
            {
                _logger.LogError(
                    "Failed to load Zone Map Details. {ZoneMapDetailsFilePath}",
                    GetZoneMapDetailsFileName()
                );
                return default(ZoneMapDetails);
            }
            return await _fileLoader.GetFile<ZoneMapDetails>(
                GetZoneMapDetailsFileName()
            );
        }

        private bool DoesZoneMapDetailsFileNotExist()
        {
            return !File.Exists(
                GetZoneMapDetailsFileName()
            );
        }

        private string GetZoneMapDetailsFileName()
        {
            return Path.Combine(
                _serverInfo.AppDataPath,
                "Map.state.json"
            );
        }

        public struct ZoneMapDetails : IMapDetails
        {
            public int Dimensions { get; set; }
            public int TileDimensions { get; set; }
            public ZoneMapMesh Mesh { get; set; }
        }

        public class ZoneMapMesh : IMapMesh
        {
            public string HeightMapUrl { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Subdivisions { get; set; }
            public int MinHeight { get; set; }
            public int MaxHeight { get; set; }
            public bool Updatable { get; set; }
            public bool IsPickable { get; set; }
        }
    }
}