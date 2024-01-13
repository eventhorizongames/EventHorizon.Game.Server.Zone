namespace EventHorizon.Zone.System.Agent.Events.Update;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Model;

using MediatR;

public struct AgentUpdateEntityCommand : IRequest
{
    public AgentEntity Agent { get; }
    public AgentAction UpdateAction { get; }

    public AgentUpdateEntityCommand(
        AgentEntity agent,
        AgentAction updateAction
    )
    {
        Agent = agent;
        UpdateAction = updateAction;
    }
}
