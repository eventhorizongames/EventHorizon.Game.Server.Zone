using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Particle.Event;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Load
{
    public class LoadCombatSystemHandler : INotificationHandler<LoadCombatSystemEvent>
    {
        readonly IMediator _mediator;

        public LoadCombatSystemHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(LoadCombatSystemEvent notification, CancellationToken cancellationToken)
        {
            // Load Particles
            await _mediator.Publish(
                new LoadCombatParticleSystemEvent()
            );
        }
    }
}