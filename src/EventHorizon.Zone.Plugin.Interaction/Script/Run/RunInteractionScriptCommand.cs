using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Model.Player;
using EventHorizon.Zone.Plugin.Interaction.Model;
using EventHorizon.Zone.Plugin.Interaction.Script.Api;
using MediatR;

namespace EventHorizon.Zone.Plugin.Interaction.Script.Run
{
    public struct RunInteractionScriptCommand : IRequest<RunInteractionScriptResponse>
    {
        public InteractionItem Interaction { get; }
        public IObjectEntity InteractionEntity { get; }
        public PlayerEntity Player { get; }
        public RunInteractionScriptCommand(
            InteractionItem interactionItem,
            IObjectEntity interactionEntity,
            PlayerEntity player
        )
        {
            Interaction = interactionItem;
            InteractionEntity = interactionEntity;
            Player = player;
        }
    }
}