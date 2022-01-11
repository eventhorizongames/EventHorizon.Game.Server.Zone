namespace EventHorizon.Zone.System.ClientEntities.Unregister;

using EventHorizon.Zone.Core.Events.Map.Cost;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.ClientEntities.Client.Delete;
using EventHorizon.Zone.System.ClientEntities.Model;
using EventHorizon.Zone.System.ClientEntities.Model.Client;
using EventHorizon.Zone.System.ClientEntities.State;

using global::System.Numerics;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class UnregisterClientEntityHandler
    : IRequestHandler<UnregisterClientEntity, bool>
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;
    private readonly ClientEntityRepository _repository;

    public UnregisterClientEntityHandler(
        ISender sender,
        IPublisher publisher,
        ClientEntityRepository repository
    )
    {
        _sender = sender;
        _publisher = publisher;
        _repository = repository;
    }

    public async Task<bool> Handle(
        UnregisterClientEntity request,
        CancellationToken cancellationToken
    )
    {
        var entity = _repository.Find(
            request.GlobalId
        );
        if (!entity.IsFound())
        {
            return false;
        }
        // If Dense remove cost from nodes/edges 
        if (entity.ContainsProperty(
            nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense)
        ) && entity.GetProperty<bool>(
            nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense)
        ))
        {
            if (entity.ContainsProperty(
                nameof(ClientEntityMetadataTypes.TYPE_DETAILS.densityBox)
            ))
            {
                await _sender.Send(
                    new RemoveEdgeCostForNodesAtPosition(
                        entity.Transform.Position,
                        entity.GetProperty<Vector3>(
                            nameof(ClientEntityMetadataTypes.TYPE_DETAILS.densityBox)
                        ),
                        500
                    ),
                    cancellationToken
                );
            }
            else
            {
                await _sender.Send(
                    new RemoveEdgeCostForNodeAtPosition(
                        entity.Transform.Position,
                        500
                    ),
                    cancellationToken
                );
            }
        }

        _repository.Remove(
            request.GlobalId
        );

        await _publisher.Publish(
            SendClientEntityDeletedClientActionToAllEvent.Create(
                new ClientEntityDeletedClientActionData(
                    request.GlobalId
                )
            ),
            cancellationToken
        );

        return true;
    }
}
