using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Model.Player;

namespace EventHorizon.Zone.Plugin.Interaction.Script.Api
{
    public interface InteractionScript
    {
        string Id { get; }
        Task<bool> Run(
            PlayerEntity player,
            IObjectEntity target,
            IDictionary<string, object> data
        );
    }
}