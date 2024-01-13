namespace EventHorizon.Zone.System.Agent.Events.Delete;

using MediatR;

public struct DeleteAgentEntityCommand
    : IRequest<DeleteAgentEntityResponse>
{
    public string AgentEntityId { get; }

    public DeleteAgentEntityCommand(
        string agentEntityId
    )
    {
        AgentEntityId = agentEntityId;
    }
}

public struct DeleteAgentEntityResponse
{
    public bool Success { get; }
    public string ErrorCode { get; }

    public DeleteAgentEntityResponse(
        bool success
    ) : this(string.Empty)
    {
        Success = success;
    }

    public DeleteAgentEntityResponse(
        string errorCode
    )
    {
        Success = false;
        ErrorCode = errorCode;
    }
}
