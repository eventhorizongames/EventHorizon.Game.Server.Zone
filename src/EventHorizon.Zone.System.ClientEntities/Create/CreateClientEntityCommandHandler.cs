namespace EventHorizon.Zone.System.ClientEntities.Create
{
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.Save;

    using global::System;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class CreateClientEntityCommandHandler : IRequestHandler<CreateClientEntityCommand, CreateClientEntityResponse>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public CreateClientEntityCommandHandler(
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<CreateClientEntityResponse> Handle(
            CreateClientEntityCommand request,
            CancellationToken cancellationToken
        )
        {
            var clientEntity = request.ClientEntity;
            // Update Id
            clientEntity.GlobalId = Guid.NewGuid().ToString();

            // Update File name
            clientEntity.RawData[ClientEntityConstants.METADATA_FILE_FULL_NAME] = Path.Combine(
                _serverInfo.ClientEntityPath,
                $"{clientEntity.ClientEntityId}.json"
            );

            await _mediator.Send(
                new SaveClientEntityCommand(
                    clientEntity
                )
            );

            return new CreateClientEntityResponse(
                true,
                clientEntity
            );
        }
    }
}
