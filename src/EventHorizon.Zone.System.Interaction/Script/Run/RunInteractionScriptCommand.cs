using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Interaction.Model;
using EventHorizon.Zone.System.Interaction.Script.Api;

using MediatR;

namespace EventHorizon.Zone.System.Interaction.Script.Run
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
