namespace EventHorizon.Zone.System.Agent.Delete
{
    using EventHorizon.Zone.System.Agent.Events.Delete;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class DeleteAgentEntityCommandHandler
        : IRequestHandler<DeleteAgentEntityCommand, DeleteAgentEntityResponse>
    {
        public Task<DeleteAgentEntityResponse> Handle(
            DeleteAgentEntityCommand request, 
            CancellationToken cancellationToken
        )
        {
            return new DeleteAgentEntityResponse("Not Implemented")
                .FromResult();
        }
    }
}
