using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;
using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Agent.Move
{
    public struct IsAgentMoving : IRequest<bool>
    {
        public long EntityId { get; }

        public IsAgentMoving(
            long entityId
        )
        {
            EntityId = entityId;
        }
        public struct IsAgentMovingHandler : IRequestHandler<IsAgentMoving, bool>
        {
            readonly IAgentRepository _agentRepository;
            public IsAgentMovingHandler(
                IAgentRepository agentRepository
            )
            {
                _agentRepository = agentRepository;
            }
            public async Task<bool> Handle(
                IsAgentMoving request,
                CancellationToken cancellationToken
            )
            {
                return (await _agentRepository.FindById(
                    request.EntityId
                )).Path?.Count > 0;
            }
        }
    }
}