
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Load.Map;
using EventHorizon.Game.Server.Zone.Load.Map.Model;
using EventHorizon.Game.Server.Zone.Loop.Map;
using EventHorizon.Game.Server.Zone.Loop.State;
using EventHorizon.Game.Server.Zone.Map;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Entity.Search.Handler
{
    public class EntitySearchMapCreatedHandler : INotificationHandler<MapCreatedEvent>
    {
        readonly ZoneMap _zoneMap;
        readonly IEntitySearchTree _searchTree;

        public EntitySearchMapCreatedHandler(IZoneMapFactory zoneMapFactory, IEntitySearchTree searchTree)
        {
            _zoneMap = zoneMapFactory.Map;
            _searchTree = searchTree;
        }

        public Task Handle(MapCreatedEvent notification, CancellationToken cancellationToken)
        {
            _searchTree.UpdateDimensions(GetDimensionsAsVector3(_zoneMap.Dimensions, _zoneMap.TileDimensions));
            return Task.CompletedTask;
        }

        private static Vector3 GetDimensionsAsVector3(int dim, int tileDim)
        {
            return new Vector3(dim * tileDim, dim * tileDim, dim * tileDim);
        }
    }
}