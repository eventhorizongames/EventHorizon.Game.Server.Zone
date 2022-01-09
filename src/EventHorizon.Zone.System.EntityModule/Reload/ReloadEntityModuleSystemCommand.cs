namespace EventHorizon.Zone.System.EntityModule.Reload;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record ReloadEntityModuleSystemCommand
    : IRequest<StandardCommandResult>;
