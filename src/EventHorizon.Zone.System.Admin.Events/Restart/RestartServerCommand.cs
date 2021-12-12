namespace EventHorizon.Zone.System.Admin.Restart;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record RestartServerCommand()
    : IRequest<StandardCommandResult>;
