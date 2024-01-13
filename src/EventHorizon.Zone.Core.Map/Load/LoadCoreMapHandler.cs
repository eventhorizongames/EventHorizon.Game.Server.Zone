namespace EventHorizon.Zone.Core.Map.Load
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map.Client;
    using EventHorizon.Zone.Core.Events.Map.Create;
    using EventHorizon.Zone.Core.Map.State;
    using EventHorizon.Zone.Core.Model.Map.Client;
    using MediatR;

    public class LoadCoreMapHandler : IRequestHandler<LoadCoreMap>
    {
        private readonly IMediator _mediator;
        private readonly IServerMap _serverMap;

        public LoadCoreMapHandler(IMediator mediator, IServerMap serverMap)
        {
            _mediator = mediator;
            _serverMap = serverMap;
        }

        public async Task Handle(LoadCoreMap request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CreateMap());

            await _mediator.Publish(
                ClientActionCoreMapLoadedToAllEvent.Create(
                    new CoreMapLoadedClientData(_serverMap.Map(), _serverMap.MapMesh())
                )
            );
        }
    }
}
