using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Particle.Add;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Particle.Add.Handler
{
    public class AddParticleTemplateHandler : INotificationHandler<AddParticleTemplateEvent>
    {
        readonly ParticleState _particleState;
        public AddParticleTemplateHandler(
            ParticleState particleState
        )
        {
            _particleState = particleState;
        }
        public Task Handle(AddParticleTemplateEvent notification, CancellationToken cancellationToken)
        {
            _particleState.Add(notification.Id, notification.Template);
            
            return Task.CompletedTask;
        }
    }
}