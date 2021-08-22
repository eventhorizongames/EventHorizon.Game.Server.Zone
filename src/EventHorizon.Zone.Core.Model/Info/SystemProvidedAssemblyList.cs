namespace EventHorizon.Zone.Core.Model.Info
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface SystemProvidedAssemblyList
    {
        IList<Assembly> List { get; }
    }
}
