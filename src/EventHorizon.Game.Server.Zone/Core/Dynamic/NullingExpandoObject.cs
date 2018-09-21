using System.Collections.Generic;
using System.Dynamic;

namespace EventHorizon.Game.Server.Zone.Core.Dynamic
{
    public class NullingExpandoObject : DynamicObject
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            values.TryGetValue(binder.Name, out result);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            values[binder.Name] = value;
            return true;
        }
    }
}