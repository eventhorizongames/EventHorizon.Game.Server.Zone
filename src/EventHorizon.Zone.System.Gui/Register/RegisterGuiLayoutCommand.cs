namespace EventHorizon.Zone.System.Gui.Register;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Gui.Model;

using MediatR;

public struct RegisterGuiLayoutCommand
    : IRequest<StandardCommandResult>
{
    public GuiLayout Layout { get; }

    public RegisterGuiLayoutCommand(
        GuiLayout layout
    )
    {
        Layout = layout;
    }
}
