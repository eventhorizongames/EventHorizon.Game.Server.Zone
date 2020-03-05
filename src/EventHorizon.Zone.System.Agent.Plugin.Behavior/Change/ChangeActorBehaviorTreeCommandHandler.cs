using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Update;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Register;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Change
{
    public class ChangeActorBehaviorTreeCommandHandler : IRequestHandler<ChangeActorBehaviorTreeCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly ActorBehaviorTreeRepository _actorBehaviorTreeRepository;

        public ChangeActorBehaviorTreeCommandHandler(
            IMediator mediator,
            ActorBehaviorTreeRepository actorBehaviorTreeRepository
        )
        {
            _mediator = mediator;
            _actorBehaviorTreeRepository = actorBehaviorTreeRepository;
        }

        public async Task<bool> Handle(
            ChangeActorBehaviorTreeCommand request,
            CancellationToken cancellationToken
        )
        {
            // Validate actor was found
            var actor = request.Entity;
            if (actor == null || !actor.IsFound())
            {
                return false;
            }

            // Validate Behavior Tree is Valid
            var treeShape = _actorBehaviorTreeRepository.FindTreeShape(
                request.NewBehaviorTreeId
            );
            if (!treeShape.IsValid)
            {
                return false;
            }

            var agentBehavior = actor.GetProperty<AgentBehavior>(
                AgentBehavior.PROPERTY_NAME
            );

            // Unregister current behavior tree to actor
            _actorBehaviorTreeRepository.UnRegisterActor(
                actor.Id
            );

            // Validate new behavior tree id exists
            var newBehaviorTreeShape = _actorBehaviorTreeRepository.FindTreeShape(
                request.NewBehaviorTreeId
            );
            if (!newBehaviorTreeShape.IsValid)
            {
                return false;
            }

            // Set actor behavior tree state to default, clearing it out.
            actor.SetProperty<BehaviorTreeState>(
                BehaviorTreeState.PROPERTY_NAME,
                new BehaviorTreeState(
                    newBehaviorTreeShape
                )
            );

            // Update Actor Agent Behavior state
            agentBehavior.TreeId = request.NewBehaviorTreeId;
            actor.SetProperty(
                AgentBehavior.PROPERTY_NAME,
                agentBehavior
            );
            await _mediator.Send(
                new UpdateEntityCommand(
                    EntityAction.PROPERTY_CHANGED,
                    actor
                )
            );

            // Register actor with new behavior tree
            await _mediator.Send(
                new RegisterActorWithBehaviorTreeUpdate(
                    actor.Id,
                    request.NewBehaviorTreeId
                )
            );

            return true;
        }
    }
}