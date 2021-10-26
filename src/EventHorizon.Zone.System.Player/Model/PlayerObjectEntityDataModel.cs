namespace EventHorizon.Zone.System.Player.Model
{
    using EventHorizon.Zone.Core.Model.Entity;

    using global::System;
    using global::System.Collections.Generic;

    public class PlayerObjectEntityDataModel
        : Dictionary<string, object>,
        ObjectEntityData
    {
        public IEnumerable<string> ForceSet
        {
            get
            {
                if (TryGetValue(
                    "forceSet",
                    out var forceSet
                ))
                {
                    var result = forceSet.To<List<string>>();
                    if (result.IsNotNull())
                    {
                        return result;
                    }
                }

                return Array.Empty<string>();
            }
        }
    }
}
