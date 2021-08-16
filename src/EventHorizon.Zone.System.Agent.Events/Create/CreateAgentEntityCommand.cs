namespace EventHorizon.Zone.System.Agent.Events.Create
{
    using EventHorizon.Zone.System.Agent.Model;

    using MediatR;

    public struct CreateAgentEntityCommand
        : IRequest<CreateAgentEntityResponse>
    {
        public AgentEntity AgentEntity { get; }

        public CreateAgentEntityCommand(
            AgentEntity agentEntity
        )
        {
            AgentEntity = agentEntity;
        }
    }

    public struct CreateAgentEntityResponse
    {
        public bool Success { get; }
        public string ErrorCode { get; }
        public AgentEntity AgentEntity { get; }

        public CreateAgentEntityResponse(
            bool success,
            AgentEntity clientEntity
        ) : this(string.Empty)
        {
            Success = success;
            AgentEntity = clientEntity;
        }

        public CreateAgentEntityResponse(
            string errorCode
        )
        {
            Success = false;
            ErrorCode = errorCode;
            AgentEntity = default;
        }
    }
}
