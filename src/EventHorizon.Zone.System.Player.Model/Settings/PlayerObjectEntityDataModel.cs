namespace EventHorizon.Zone.System.Player.Model.Settings;

using global::System;
using EventHorizon.Zone.Core.Model.Entity;
using global::System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class PlayerObjectEntityDataModel : Dictionary<string, object>, ObjectEntityData
{
    private IEnumerable<string>? _forceSet;
    public IEnumerable<string> ForceSet
    {
        get
        {
            if (_forceSet.IsNotNull())
            {
                return _forceSet;
            }
            else if (TryGetValue("forceSet", out var forceSet) && forceSet is JArray forceSetArray)
            {
                _forceSet = forceSetArray.ToObject<List<string>>();
            }

            return _forceSet ?? [];
        }
    }
}
