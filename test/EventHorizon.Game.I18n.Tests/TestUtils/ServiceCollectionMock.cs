namespace EventHorizon.Game.I18n.Tests.TestUtils
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Extensions.DependencyInjection;

    public class ServiceCollectionMock : IServiceCollection
    {
        public Dictionary<int, ServiceDescriptor> Services = new Dictionary<int, ServiceDescriptor>();

        public ServiceDescriptor this[int index] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public int Count => Services.Count;

        public bool IsReadOnly => throw new System.NotImplementedException();

        public void Add(ServiceDescriptor item)
        {
            Services.Add(Services.Count + 1, item);
        }

        public void Clear()
        {
            Services.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            return Services.Values.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            Services.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return Services.Values.GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(ServiceDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Services.Values.GetEnumerator();
        }
    }
}
