namespace EventHorizon.Zone.Core.Info
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;

    using EventHorizon.Zone.Core.Model.Info;

    public class StandardSystemProvidedAssemblyList : SystemProvidedAssemblyList
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
