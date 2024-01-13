namespace EventHorizon.Zone.System.DataStorage.Events.Query;

using EventHorizon.Zone.Core.Model.Command;

using global::System.Collections.Generic;

using MediatR;

public class QueryForAllDataStoreValues
    : IRequest<CommandResult<IReadOnlyDictionary<string, object>>>
{
}
