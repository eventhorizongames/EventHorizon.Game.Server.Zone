namespace EventHorizon.Zone.System.Particle.Reload;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Particle.ClientActions;
using EventHorizon.Zone.System.Particle.Load;
using EventHorizon.Zone.System.Particle.Query;
using EventHorizon.Zone.System.Particle.State;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class ReloadParticleSystemCommandHandler
    : IRequestHandler<
          ReloadParticleSystemCommand,
          StandardCommandResult
      >
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;
    private readonly ParticleTemplateRepository _repository;

    public ReloadParticleSystemCommandHandler(
        ISender sender,
        IPublisher publisher,
        ParticleTemplateRepository repository
    )
    {
        _sender = sender;
        _publisher = publisher;
        _repository = repository;
    }

    public async Task<StandardCommandResult> Handle(
        ReloadParticleSystemCommand request,
        CancellationToken cancellationToken
    )
    {
        _repository.Clear();

        await _publisher.Publish(
            new LoadParticleSystemEvent(),
            cancellationToken
        );

        await _publisher.Publish(
            ParticleSystemReloadedClientActionToAllEvent.Create(
                await _sender.Send(
                    new QueryForAllParticleTemplates(),
                    cancellationToken
                )
            ),
            cancellationToken
        );

        return new StandardCommandResult();
    }
}
