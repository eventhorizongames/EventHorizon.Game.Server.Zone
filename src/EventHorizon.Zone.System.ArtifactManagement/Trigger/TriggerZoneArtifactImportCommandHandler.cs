namespace EventHorizon.Zone.System.ArtifactManagement.Trigger;

using EventHorizon.BackgroundTasks.Queue;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ArtifactManagement.Tasks;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class TriggerZoneArtifactImportCommandHandler
    : IRequestHandler<TriggerZoneArtifactImportCommand, CommandResult<TriggerZoneArtifactImportResult>>
{
    private readonly ISender _sender;

    public TriggerZoneArtifactImportCommandHandler(
        ISender sender
    )
    {
        _sender = sender;
    }

    public Task<CommandResult<TriggerZoneArtifactImportResult>> Handle(
        TriggerZoneArtifactImportCommand request,
        CancellationToken cancellationToken
    )
    {
        var backgroundTask = new ImportZoneDataTask(
            request.ImportArtifactUrl
        );

        _sender.Send(
            new EnqueueBackgroundJob(
                backgroundTask
            ),
            cancellationToken
        );

        return new CommandResult<TriggerZoneArtifactImportResult>(
            new TriggerZoneArtifactImportResult(
                backgroundTask.ReferenceId
            )
        ).FromResult();
    }
}
