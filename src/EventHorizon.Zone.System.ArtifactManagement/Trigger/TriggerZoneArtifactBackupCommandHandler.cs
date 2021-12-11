namespace EventHorizon.Zone.System.ArtifactManagement.Trigger;

using EventHorizon.BackgroundTasks.Queue;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ArtifactManagement.Tasks;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class TriggerZoneArtifactBackupCommandHandler
    : IRequestHandler<TriggerZoneArtifactBackupCommand, CommandResult<TriggerZoneArtifactBackupResult>>
{
    private readonly ISender _sender;

    public TriggerZoneArtifactBackupCommandHandler(
        ISender sender
    )
    {
        _sender = sender;
    }

    public Task<CommandResult<TriggerZoneArtifactBackupResult>> Handle(
        TriggerZoneArtifactBackupCommand request,
        CancellationToken cancellationToken
    )
    {
        var backgroundTask = new BackupZoneDataTask();

        _sender.Send(
            new EnqueueBackgroundJob(
                backgroundTask
            ),
            cancellationToken
        );

        return new CommandResult<TriggerZoneArtifactBackupResult>(
            new TriggerZoneArtifactBackupResult(
                backgroundTask.ReferenceId
            )
        ).FromResult();
    }
}
