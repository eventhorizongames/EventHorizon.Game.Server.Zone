namespace EventHorizon.Zone.Core.Map.Find
{
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Events.Path;
    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;

    public class FindPathHandler 
        : IRequestHandler<FindPathEvent, Queue<Vector3>>
    {
        private readonly IMediator _mediator;
        private readonly IMapGraph _map;
        private readonly PathFindingAlgorithm _pathFinding;

        public FindPathHandler(
            IMediator mediator,
            IMapGraph map,
            PathFindingAlgorithm pathFinding
        )
        {
            _mediator = mediator;
            _map = map;
            _pathFinding = pathFinding;
        }

        public async Task<Queue<Vector3>> Handle(
            FindPathEvent request,
            CancellationToken cancellationToken
        )
        {
            var fromMapNode = await _mediator.Send(
                new GetMapNotDenseNodeAtPosition(
                    request.From
                )
            );
            var toMapNode = await _mediator.Send(
                new GetMapNotDenseNodeAtPosition(
                    request.To
                )
            );

            return _pathFinding.Search(
                _map,
                fromMapNode,
                toMapNode
            );
        }
    }
}