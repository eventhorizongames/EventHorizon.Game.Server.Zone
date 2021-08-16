namespace EventHorizon.Zone.System.Agent.Delete
{
    using EventHorizon.Zone.System.Agent.Events.Delete;
    using EventHorizon.Zone.System.Agent.Events.Register;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class DeleteAgentEntityCommandHandler
        : IRequestHandler<DeleteAgentEntityCommand, DeleteAgentEntityResponse>
    {
        private readonly IMediator _mediator;

        public DeleteAgentEntityCommandHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<DeleteAgentEntityResponse> Handle(
            DeleteAgentEntityCommand request,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new UnRegisterAgent(
                    request.AgentEntityId
                ),
                cancellationToken
            );

            return new DeleteAgentEntityResponse(true);
        }
    }
}
