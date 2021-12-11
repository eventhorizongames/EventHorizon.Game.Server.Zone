namespace EventHorizon.Zone.System.ArtifactManagement.Tasks;

using EventHorizon.BackgroundTasks.Model;
using EventHorizon.Zone.System.ArtifactManagement.Backup;
using EventHorizon.Zone.System.ArtifactManagement.ClientActions;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class BackupZoneDataTaskHandler
    : BackgroundTaskHandler<BackupZoneDataTask>
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;

    public BackupZoneDataTaskHandler(
        ISender sender,
        IPublisher publisher
    )
    {
        _sender = sender;
        _publisher = publisher;
    }

    public async Task<BackgroundTaskResult> Handle(
        BackupZoneDataTask request,
        CancellationToken cancellationToken
    )
    {
        var result = await _sender.Send(
            new BackupZoneDataCommand(
                request.ReferenceId
            ),
            cancellationToken
        );

        if (!result)
        {
            await _publisher.Publish(
                AdminClientActionFailedZoneServerBackupEvent.Create(
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
            AdminClientActionFinishedZoneServerBackupEvent.Create(
                request.ReferenceId,
                result.Result.Path
            ),
            cancellationToken
        );

        return new();
    }
}
