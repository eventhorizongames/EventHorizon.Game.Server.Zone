using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace EventHorizon.Zone.Core.Model.Info
{
    public struct StandardSystemProvidedAssemblyList : SystemProvidedAssemblyList
    {
        public IList<Assembly> List { get; }
        public StandardSystemProvidedAssemblyList(
            Assembly[] list
        )
        {
            List = new ReadOnlyCollection<Assembly>(
                list
            );
        }
    }
}