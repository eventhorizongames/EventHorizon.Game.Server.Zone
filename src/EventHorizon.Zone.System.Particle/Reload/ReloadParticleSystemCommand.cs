namespace EventHorizon.Zone.System.Particle.Reload;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public struct ReloadParticleSystemCommand
    : IRequest<StandardCommandResult> { }
