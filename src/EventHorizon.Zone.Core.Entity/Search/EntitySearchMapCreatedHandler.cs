

using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Events.Map.Create;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Zone.Core.Entity.Search
{
    public class EntitySearchMapCreatedHandler : INotificationHandler<MapCreatedEvent>
    {
        readonly IMapDetails _mapDetails;
        readonly EntitySearchTree _searchTree;

        public EntitySearchMapCreatedHandler(
            IMapDetails mapDetails,
            EntitySearchTree searchTree
        )
        {
            _mapDetails = mapDetails;
            _searchTree = searchTree;
        }

        public Task Handle(
            MapCreatedEvent notification,
            CancellationToken cancellationToken
        )
        {
            _searchTree.UpdateDimensions(
                GetDimensionsAsVector3(
                    _mapDetails.Dimensions,
                    _mapDetails.TileDimensions
                )
            );
            return Task.CompletedTask;
        }

        private static Vector3 GetDimensionsAsVector3(
            int dim,
            int tileDim
        )
        {
            return new Vector3(
                dim * tileDim,
                dim * tileDim,
                dim * tileDim
            );
        }
    }
}