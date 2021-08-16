namespace EventHorizon.Zone.System.Agent.Create
{
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.Create;
    using EventHorizon.Zone.System.Agent.Events.Register;

    using global::System;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class CreateAgentEntityCommandHandler
        : IRequestHandler<CreateAgentEntityCommand, CreateAgentEntityResponse>
    {
        private readonly IMediator _mediator;

        public CreateAgentEntityCommandHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<CreateAgentEntityResponse> Handle(
            CreateAgentEntityCommand request,
            CancellationToken cancellationToken
        )
        {
            var entity = request.AgentEntity;
            entity.AgentId = Guid.NewGuid().ToString();
            entity.Type = EntityType.AGENT;
            // Remove any existing LocationState, New entities will get it recreated.
            entity.Data.TryRemove(LocationState.PROPERTY_NAME, out _);

            var newEntity = await _mediator.Send(
                new RegisterAgentEvent(
                    entity
                ),
                cancellationToken
            );

            return new CreateAgentEntityResponse(
                true,
                newEntity
            );
        }
    }
}
