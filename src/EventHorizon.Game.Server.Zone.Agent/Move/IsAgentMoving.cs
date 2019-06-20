using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using MediatR;

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
            readonly IMoveAgentRepository _repository;
            public IsAgentMovingHandler(
                IMoveAgentRepository repository
            )
            {
                _repository = repository;
            }
            public Task<bool> Handle(
                IsAgentMoving request,
                CancellationToken cancellationToken
            )
            {
                return Task.FromResult(
                    _repository.Contains(
                        request.EntityId
                    )
                );
            }
        }
    }
}