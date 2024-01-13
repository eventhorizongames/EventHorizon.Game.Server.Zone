namespace EventHorizon.Zone.System.Admin.ExternalHub;

using EventHorizon.Zone.System.Agent.Events.Create;
using EventHorizon.Zone.System.Agent.Events.Delete;
using EventHorizon.Zone.System.Agent.Events.Save;
using EventHorizon.Zone.System.Agent.Model;

using global::System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Make sure this stays on the ExternalHub root namespace.
/// This Class is encapsulating the Command related logic,
///  and allows for a single SignalR hub to host all APIs.
/// </summary>
public partial class AdminHub
    : Hub
{
    public Task<SaveAgentEntityResponse> Agent_EntitySave(
        AgentEntity agentEntity
    ) => _mediator.Send(
        new SaveAgentEntityCommand(
            agentEntity
        ),
        Context.ConnectionAborted
    );

    public Task<CreateAgentEntityResponse> Agent_EntityCreate(
        AgentEntity agentEntity
    ) => _mediator.Send(
        new CreateAgentEntityCommand(
            agentEntity
        ),
        Context.ConnectionAborted
    );

    public Task<DeleteAgentEntityResponse> Agent_EntityDelete(
        string agentEntityId
    ) => _mediator.Send(
        new DeleteAgentEntityCommand(
            agentEntityId
        ),
        Context.ConnectionAborted
    );
}
