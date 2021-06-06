namespace EventHorizon.Zone.System.DataStorage.Save
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System;
    using MediatR;

    public struct SaveDataStoreCommand
        : IRequest<StandardCommandResult>
    {
    }
}
