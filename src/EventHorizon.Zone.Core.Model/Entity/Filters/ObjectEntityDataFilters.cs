namespace EventHorizon.Zone.Core.Model.Entity.Filters;

using System.Collections.Generic;
using System.Linq;

public static class ObjectEntityDataFilters
{
    public static IEnumerable<KeyValuePair<string, object>> FilterOutForceSetKey(
        this ObjectEntityData objectEntityData
    ) => objectEntityData.Where(
        property => !property.Key.StartsWith(
            nameof(ObjectEntityData.ForceSet).LowercaseFirstChar()
        )
    );
}
