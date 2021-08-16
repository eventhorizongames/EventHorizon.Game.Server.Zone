namespace EventHorizon.Zone.System.ClientEntities.PopulateData
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.ClientEntities.Model;

    using global::System;
    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class PopulateClientEntityDataEventHandler : INotificationHandler<PopulateClientEntityDataEvent>
    {
        public Task Handle(
            PopulateClientEntityDataEvent notification,
            CancellationToken cancellationToken
        )
        {
            var entity = notification.ClientEntity;

            // Populate Data for "assetId"
            entity.PopulateData<string>(
                nameof(ClientEntityMetadataTypes.assetId),
                setDefaultValue: false
            );

            // Populate Data for "dense"
            entity.PopulateData<bool>(
                nameof(ClientEntityMetadataTypes.dense),
                setDefaultValue: false
            );

            // Populate Data for "densityBox"
            entity.PopulateData<Nullable<Vector3>>(
                nameof(ClientEntityMetadataTypes.densityBox),
                setDefaultValue: false
            );

            // Populate Data for "resolveHeight"
            entity.PopulateData<bool>(
                nameof(ClientEntityMetadataTypes.resolveHeight),
                setDefaultValue: false
            );

            // Populate Data for "heightOffset"
            entity.PopulateData<long>(
                nameof(ClientEntityMetadataTypes.heightOffset),
                setDefaultValue: false
            );


            return Task.CompletedTask;
        }
    }
}
