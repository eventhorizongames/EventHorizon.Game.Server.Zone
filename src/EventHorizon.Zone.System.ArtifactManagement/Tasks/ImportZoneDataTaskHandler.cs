namespace EventHorizon.Zone.System.ArtifactManagement.Tasks;

using EventHorizon.BackgroundTasks.Model;
using EventHorizon.Zone.System.ArtifactManagement.ClientActions;
using EventHorizon.Zone.System.ArtifactManagement.Import;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class ImportZoneDataTaskHandler
    : BackgroundTaskHandler<ImportZoneDataTask>
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;

    public ImportZoneDataTaskHandler(
        ISender sender,
        IPublisher publisher
    )
    {
        _sender = sender;
        _publisher = publisher;
    }

    public async Task<BackgroundTaskResult> Handle(
        ImportZoneDataTask request,
        CancellationToken cancellationToken
    )
    {
        var result = await _sender.Send(
            new ImportZoneDataCommand(
                request.ReferenceId,
                request.ImportArtifactUrl
            ),
            cancellationToken
        );

        if (!result)
        {
            await _publisher.Publish(
                AdminClientActionFailedZoneServerImportEvent.Create(
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
            AdminClientActionFinishedZoneServerImportEvent.Create(
                request.ReferenceId
            ),
            cancellationToken
        );
        await _publisher.Publish(
            ClientActionFinishedZoneServerImportEvent.Create(
                request.ReferenceId
            ),
            cancellationToken
        );

        return new();
    }
}
