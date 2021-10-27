namespace EventHorizon.Zone.Core.Entity.Model
{
    using System;
    using System.Collections.Generic;

    using EventHorizon.Zone.Core.Model.Entity;

    using Newtonsoft.Json.Linq;

    public class ObjectEntityDataModel
        : Dictionary<string, object>,
        ObjectEntityData
    {
        private IEnumerable<string>? _forceSet;
        public IEnumerable<string> ForceSet
        {
            get
            {
                if (_forceSet != null)
                {
                    return _forceSet;
                }
                else if (TryGetValue(
                    "forceSet",
                    out var forceSet
                ) && forceSet is JArray forceSetArray)
                {
                    _forceSet = forceSetArray.ToObject<List<string>>();
                }

                return _forceSet ?? Array.Empty<string>();
            }
        }
    }
}
