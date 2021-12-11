namespace EventHorizon.Zone.System.ArtifactManagement.Tasks;

using EventHorizon.BackgroundTasks.Model;
using EventHorizon.Zone.System.ArtifactManagement.ClientActions;
using EventHorizon.Zone.System.ArtifactManagement.Export;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class ExportZoneDataTaskHandler
    : BackgroundTaskHandler<ExportZoneDataTask>
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;

    public ExportZoneDataTaskHandler(
        ISender sender,
        IPublisher publisher
    )
    {
        _sender = sender;
        _publisher = publisher;
    }

    public async Task<BackgroundTaskResult> Handle(
        ExportZoneDataTask request,
        CancellationToken cancellationToken
    )
    {
        var result = await _sender.Send(
            new ExportZoneDataCommand(
                request.ReferenceId
            ),
            cancellationToken
        );

        if (!result)
        {
            await _publisher.Publish(
                AdminClientActionFailedZoneServerExportEvent.Create(
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
            AdminClientActionFinishedZoneServerExportEvent.Create(
                request.ReferenceId,
                result.Result.Path
            ),
            cancellationToken
        );
        
        return new();
    }
}
