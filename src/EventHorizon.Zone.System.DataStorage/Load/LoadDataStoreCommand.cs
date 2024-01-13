namespace EventHorizon.Zone.System.DataStorage.Load;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public struct LoadDataStoreCommand
    : IRequest<StandardCommandResult>
{
}
