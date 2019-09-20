using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Player;
using EventHorizon.Zone.System.Gui.Model;
using MediatR;

namespace EventHorizon.Zone.System.Gui.Events.Layout
{
    public struct GetGuiLayoutListForPlayerCommand : IRequest<IEnumerable<GuiLayout>>
    {
        public PlayerEntity Player { get; }
        public GetGuiLayoutListForPlayerCommand(
            PlayerEntity player
        )
        {
            Player = player;
        }
    }
}