namespace EventHorizon.Zone.System.Gui.Events.Layout
{
    using global::System.Collections.Generic;

    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Gui.Model;

    using MediatR;

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
