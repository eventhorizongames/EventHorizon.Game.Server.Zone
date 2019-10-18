using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using EventHorizon.Zone.Core.Model.Info;

namespace EventHorizon.Zone.Core.Info
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