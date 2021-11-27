namespace EventHorizon.Zone.System.Template.Reload;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public struct ReloadTemplateSystemCommand
    : IRequest<StandardCommandResult>
{
}
