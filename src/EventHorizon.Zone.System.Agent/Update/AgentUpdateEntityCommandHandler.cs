namespace EventHorizon.Zone.System.Agent.Update;

using EventHorizon.Zone.System.Agent.Events.Update;
using EventHorizon.Zone.System.Agent.Model.State;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;

public class AgentUpdateEntityCommandHandler : IRequestHandler<AgentUpdateEntityCommand>
{
    private readonly IAgentRepository _agentRepository;

    public AgentUpdateEntityCommandHandler(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }

    public async Task Handle(
        AgentUpdateEntityCommand request,
        CancellationToken cancellationToken
    )
    {
        await _agentRepository.Update(request.UpdateAction, request.Agent);
    }
}
