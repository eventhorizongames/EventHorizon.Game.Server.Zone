using System.Collections.Generic;
using System.Reflection;

namespace EventHorizon.Zone.Core.Model.Info
{
    public interface SystemProvidedAssemblyList
    {
        IList<Assembly> List { get; }
    }
}