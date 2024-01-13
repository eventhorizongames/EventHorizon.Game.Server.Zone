namespace EventHorizon.Zone.System.Agent.Events.Save;

using EventHorizon.Zone.System.Agent.Model;

using MediatR;

public struct SaveAgentEntityCommand
    : IRequest<SaveAgentEntityResponse>
{
    public AgentEntity AgentEntity { get; }

    public SaveAgentEntityCommand(
        AgentEntity agentEntity
    )
    {
        AgentEntity = agentEntity;
    }
}

public struct SaveAgentEntityResponse
{
    public bool Success { get; }
    public string ErrorCode { get; }
    public AgentEntity AgentEntity { get; }

    public SaveAgentEntityResponse(
        bool success,
        AgentEntity clientEntity
    ) : this(string.Empty)
    {
        Success = success;
        AgentEntity = clientEntity;
    }

    public SaveAgentEntityResponse(
        string errorCode
    )
    {
        Success = false;
        ErrorCode = errorCode;
        AgentEntity = default;
    }
}
