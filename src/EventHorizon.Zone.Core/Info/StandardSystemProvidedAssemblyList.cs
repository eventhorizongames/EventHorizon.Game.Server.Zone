namespace EventHorizon.Zone.Core.Info
{
    using EventHorizon.Zone.Core.Model.Info;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;

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