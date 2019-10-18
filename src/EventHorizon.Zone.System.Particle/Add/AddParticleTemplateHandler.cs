using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Particle.Events.Add;
using EventHorizon.Zone.System.Particle.State;
using MediatR;

namespace EventHorizon.Zone.System.Particle.Add
{
    public struct AddParticleTemplateHandler : INotificationHandler<AddParticleTemplateEvent>
    {
        readonly ParticleTemplateRepository _repository;
        public AddParticleTemplateHandler(
            ParticleTemplateRepository repository
        )
        {
            _repository = repository;
        }
        public Task Handle(
            AddParticleTemplateEvent notification,
            CancellationToken cancellationToken
        )
        {
            _repository.Add(
                notification.Id,
                notification.Template
            );

            return Task.CompletedTask;
        }
    }
}