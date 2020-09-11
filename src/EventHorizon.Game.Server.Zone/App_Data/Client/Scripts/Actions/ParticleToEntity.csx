/*
data:
    startEntityId: number
    endingEntityId: number
    particleTemplateId: string
    duration: number
*/

using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Core.Factory.Api;
using EventHorizon.Game.Client.Core.Timer.Api;
using EventHorizon.Game.Client.Engine.Entity.Tag;
using EventHorizon.Game.Client.Engine.Entity.Tracking.Query;
using EventHorizon.Game.Client.Engine.Lifecycle.Register.Dispose;
using EventHorizon.Game.Client.Engine.Particle.Create;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Client.Engine.Systems.Entity.Api;
using Microsoft.Extensions.Logging;

// TODO: Flush this out more.
public class __SCRIPT__
    : IClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(
        ScriptServices services,
        ScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Script");

        var startingEntity = await GetEntity(
            services,
            data.Get<long>("startEntityId")
        );
        var endingEntity = await GetEntity(
            services,
            data.Get<long>("endingEntityId")
        );
        var particleId = "Particle_Flame";
        var speed = 3;

        var particleEntityResult = await services.Mediator.Send(
            new CreateParticleEmitterCommand(
                particleId,
                startingEntity.Transform.Position,
                speed
            )
        );
        if (particleEntityResult.Success)
        {
            var particleEntity = particleEntityResult.Result;
            particleEntity.MoveTo(
                endingEntity.Transform.Position
            );

            var stopTimer = services.GetService<IFactory<ITimerService>>().Create();
            var disposeTimer = services.GetService<IFactory<ITimerService>>().Create();

            stopTimer.SetTimer(
                3000,
                () => particleEntity.Stop()
            );
            stopTimer.SetTimer(
                5000,
                () => services.Mediator.Send(
                    new DisposeOfEntityCommand(
                        particleEntity
                    )
                )
            );
        }

    }

    private async Task<IObjectEntity> GetEntity(
        ScriptServices services,
        long id
    )
    {

        var entities = await services.Mediator.Send(
            new QueryForEntity(
                TagBuilder.CreateEntityIdTag(
                    id.ToString()
                )
            )
        );

        return entities.Result.FirstOrDefault();
    }
}
