namespace EventHorizon.Zone.System.EntityModule.Load;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record LoadEntityModuleSystemCommand
    : IRequest<StandardCommandResult>;
