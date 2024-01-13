namespace EventHorizon.Zone.System.DataStorage.Query;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.DataStorage.Api;
using EventHorizon.Zone.System.DataStorage.Events.Query;

using global::System.Collections.Generic;
using global::System.Collections.ObjectModel;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class QueryForAllDataStoreValuesHandler
    : IRequestHandler<QueryForAllDataStoreValues, CommandResult<IReadOnlyDictionary<string, object>>>
{
    private readonly DataStoreManagement _dataStoreManagement;

    public QueryForAllDataStoreValuesHandler(
        DataStoreManagement dataStoreManagement
    )
    {
        _dataStoreManagement = dataStoreManagement;
    }

    public Task<CommandResult<IReadOnlyDictionary<string, object>>> Handle(
        QueryForAllDataStoreValues request,
        CancellationToken cancellationToken
    )
    {
        return new CommandResult<IReadOnlyDictionary<string, object>>(
            new ReadOnlyDictionary<string, object>(
                _dataStoreManagement.Data()
            )
        ).FromResult();
    }
}
