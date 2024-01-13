namespace EventHorizon.Zone.System.Agent.Save;

using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Events.Save;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Save.Mapper;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class SaveAgentEntityCommandHandler
    : IRequestHandler<SaveAgentEntityCommand, SaveAgentEntityResponse>
{
    private readonly IMediator _mediator;

    public SaveAgentEntityCommandHandler(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task<SaveAgentEntityResponse> Handle(
        SaveAgentEntityCommand request,
        CancellationToken cancellationToken
    )
    {
        var agentDetails = AgentFromEntityToDetails.Map(
            request.AgentEntity
        );
        var agentEntity = AgentFromDetailsToEntity.MapToNew(
            agentDetails,
            agentDetails.Id
        );
        await _mediator.Send(
            new UnRegisterAgent(
                agentDetails.Id
            ),
            cancellationToken
        );
        var registeredEntity = await _mediator.Send(
            new RegisterAgentEvent(
                agentEntity
            ),
            cancellationToken
        );
        return new SaveAgentEntityResponse(
            true,
            registeredEntity
        );
    }
}
