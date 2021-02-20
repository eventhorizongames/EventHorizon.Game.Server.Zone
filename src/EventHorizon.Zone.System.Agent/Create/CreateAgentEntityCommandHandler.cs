namespace EventHorizon.Zone.System.Agent.Create
{
    using EventHorizon.Zone.System.Agent.Events.Create;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class CreateAgentEntityCommandHandler
        : IRequestHandler<CreateAgentEntityCommand, CreateAgentEntityResponse>
    {
        public Task<CreateAgentEntityResponse> Handle(
            CreateAgentEntityCommand request, 
            CancellationToken cancellationToken
        )
        {
            return new CreateAgentEntityResponse("Not Implemented")
                .FromResult();
        }
    }
}
