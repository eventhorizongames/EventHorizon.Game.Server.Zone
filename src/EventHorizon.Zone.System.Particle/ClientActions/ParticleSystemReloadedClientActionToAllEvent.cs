namespace EventHorizon.Zone.System.Particle.ClientActions;

using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Zone.System.Particle.Model.Template;

using global::System.Collections.Generic;

public static class ParticleSystemReloadedClientActionToAllEvent
{
    public static ClientActionGenericToAllEvent Create(
        IEnumerable<ParticleTemplate> particleTemplateList
    ) =>
        new(
            "PARTICLE_SYSTEM_RELOADED_CLIENT_ACTION_EVENT",
            new ParticleSystemReloadedClientActionData(
                particleTemplateList
            )
        );

    public record ParticleSystemReloadedClientActionData(
        IEnumerable<ParticleTemplate> ParticleTemplateList
    ) : IClientActionData;
}
