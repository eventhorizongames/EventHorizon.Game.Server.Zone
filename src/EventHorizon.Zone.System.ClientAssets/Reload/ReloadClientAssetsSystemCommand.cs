namespace EventHorizon.Zone.System.ClientAssets.Reload;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record ReloadClientAssetsSystemCommand()
    : IRequest<StandardCommandResult>;
