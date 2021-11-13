namespace EventHorizon.Test.Common.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.Extensions.DependencyInjection;

    [ExcludeFromCodeCoverage]
    public class ServiceCollectionMock : IServiceCollection
    {
        public ServiceDescriptor[] Services = System.Array.Empty<ServiceDescriptor>();

        public ServiceDescriptor this[int index]
        {
            get
            {
                return Services[index];
            }
            set
            {
                Services[index] = value;
            }
        }

        public int Count => Services.Length;

        public bool IsReadOnly => throw new System.NotImplementedException();

        public void Add(ServiceDescriptor item)
        {
            Services = Services.InsertItem(Services.Length, item).ToArray();
        }

        public void Clear()
        {
            Services = System.Array.Empty<ServiceDescriptor>();
        }

        public bool Contains(ServiceDescriptor item)
        {
            return Services.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            Services.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return Services.ToList().GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            for (int i = 0; i < Services.Length; i++)
            {
                var service = Services[i];
                if (service == item)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            Services = Services.InsertItem(index, item).ToArray();
        }

        public bool Remove(ServiceDescriptor item)
        {
            var count = Services.Length;
            Services = Services.RemoveItem(item).ToArray();
            return Services.Length != count;
        }

        public void RemoveAt(int index)
        {
            Services = Services.Where((_, itemIndex) => itemIndex != index).ToArray();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Services.GetEnumerator();
        }
    }
}
