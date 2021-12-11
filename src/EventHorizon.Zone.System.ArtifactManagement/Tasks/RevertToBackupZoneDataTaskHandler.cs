namespace EventHorizon.Zone.System.ArtifactManagement.Tasks;

using EventHorizon.BackgroundTasks.Model;
using EventHorizon.Zone.System.ArtifactManagement.ClientActions;
using EventHorizon.Zone.System.ArtifactManagement.Revert;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class RevertToBackupZoneDataTaskHandler
    : BackgroundTaskHandler<RevertToBackupZoneDataTask>
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;

    public RevertToBackupZoneDataTaskHandler(
        ISender sender,
        IPublisher publisher
    )
    {
        _sender = sender;
        _publisher = publisher;
    }

    public async Task<BackgroundTaskResult> Handle(
        RevertToBackupZoneDataTask request,
        CancellationToken cancellationToken
    )
    {
        var result = await _sender.Send(
            new RevertToBackupZoneDataCommand(
                request.ReferenceId,
                request.BackupArtifactUrl
            ),
            cancellationToken
        );

        if (!result)
        {
            await _publisher.Publish(
                AdminClientActionFailedZoneServerRevertToBackupEvent.Create(
                    request.ReferenceId,
                    result.ErrorCode
                ),
                cancellationToken
            );
            return new BackgroundTaskResult
            {
                Success = false,
                // TODO: Add Error Code Support/Logging to BackgroundTask Processing
                //ErrorCode = result.ErrorCode,
            };
        }

        await _publisher.Publish(
            AdminClientActionFinishedZoneServerRevertToBackupEvent.Create(
                request.ReferenceId
            ),
            cancellationToken
        );
        await _publisher.Publish(
            ClientActionFinishedZoneServerRevertToBackupEvent.Create(
                request.ReferenceId
            ),
            cancellationToken
        );

        return new();
    }
}
