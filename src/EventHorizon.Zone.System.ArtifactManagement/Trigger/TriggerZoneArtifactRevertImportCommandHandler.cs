namespace EventHorizon.Zone.System.ArtifactManagement.Trigger;

using EventHorizon.BackgroundTasks.Queue;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ArtifactManagement.Tasks;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class TriggerZoneArtifactRevertImportCommandHandler
    : IRequestHandler<TriggerZoneArtifactRevertImportCommand, CommandResult<TriggerZoneArtifactRevertImportResult>>
{
    private readonly ISender _sender;

    public TriggerZoneArtifactRevertImportCommandHandler(
        ISender sender
    )
    {
        _sender = sender;
    }

    public Task<CommandResult<TriggerZoneArtifactRevertImportResult>> Handle(
        TriggerZoneArtifactRevertImportCommand request,
        CancellationToken cancellationToken
    )
    {
        var backgroundTask = new RevertToBackupZoneDataTask(
            request.BackupArtifactUrl
        );

        _sender.Send(
            new EnqueueBackgroundJob(
                backgroundTask
            ),
            cancellationToken
        );

        return new TriggerZoneArtifactRevertImportResult(
            backgroundTask.ReferenceId
        ).ToCommandResult().FromResult();
    }
}
