using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace EventHorizon.Identity.Tests.TestUtils
{
    public class ConfigurationMock : IConfiguration
    {
        private IDictionary<string, string> _data = new Dictionary<string, string>();
        public string this[string key]
        {
            get => _data[key];
            set => _data[key] = value;
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new System.NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new System.NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}