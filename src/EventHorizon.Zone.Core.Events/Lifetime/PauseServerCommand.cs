namespace EventHorizon.Zone.Core.Events.Lifetime;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record PauseServerCommand()
    : IRequest<StandardCommandResult>;
