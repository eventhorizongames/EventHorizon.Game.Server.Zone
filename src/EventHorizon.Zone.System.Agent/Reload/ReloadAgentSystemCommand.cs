namespace EventHorizon.Zone.System.Agent.Reload;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public struct ReloadAgentSystemCommand
    : IRequest<StandardCommandResult>
{
}
