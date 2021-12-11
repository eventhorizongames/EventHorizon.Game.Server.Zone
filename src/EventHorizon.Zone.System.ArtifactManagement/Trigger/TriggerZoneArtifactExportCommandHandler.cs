namespace EventHorizon.Zone.System.ArtifactManagement.Trigger;

using EventHorizon.BackgroundTasks.Queue;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ArtifactManagement.Tasks;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class TriggerZoneArtifactExportCommandHandler
    : IRequestHandler<TriggerZoneArtifactExportCommand, CommandResult<TriggerZoneArtifactExportResult>>
{
    private readonly ISender _sender;

    public TriggerZoneArtifactExportCommandHandler(
        ISender sender
    )
    {
        _sender = sender;
    }

    public Task<CommandResult<TriggerZoneArtifactExportResult>> Handle(
        TriggerZoneArtifactExportCommand request,
        CancellationToken cancellationToken
    )
    {
        var backgroundTask = new ExportZoneDataTask();

        _sender.Send(
            new EnqueueBackgroundJob(
                backgroundTask
            ),
            cancellationToken
        );

        return new CommandResult<TriggerZoneArtifactExportResult>(
            new TriggerZoneArtifactExportResult(
                backgroundTask.ReferenceId
            )
        ).FromResult();
    }
}
