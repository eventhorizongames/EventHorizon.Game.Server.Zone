namespace EventHorizon.Zone.System.Gui.Reload;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record ReloadGuiSystemCommand
    : IRequest<StandardCommandResult>;
