using System.IO;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Dynamic;
using Newtonsoft.Json;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Core.Dynamic
{
    public class ISetDataTests
    {
        [Fact]
        public void TestISetData_ShouldBeAbleToSetPropertiesOnISetDataBasedStruct()
        {
            var jsonString = @"{
                ""data"": {
                    ""Ai"": {
                        ""omeProp"": ""a String Property""
                    }
                }
            }";
            var actual = JsonConvert.DeserializeObject<AgentEntity>(jsonString);
            string props = actual.GetData<AgentEntityAi>("Ai").SomeProp;
            var ai = actual.GetData<AgentEntityAi>("Ai");
            ai.SomeProp = "other value";

            Assert.Equal("other value", ai.SomeProp);
        }
    }
    public struct AgentEntity
    {
        private dynamic _data;
        public dynamic Data
        {
            set { _data = value; }
        }
        public T GetData<T>(string prop) where T : ISetData, new()
        {
            ISetData newData = new T();
            newData.Data = _data[prop];
            return (T)newData;
        }
    }
    public struct AgentEntityAi : ISetData
    {
        private dynamic _data;
        public dynamic Data
        {
            set { _data = value; }
        }
        public string SomeProp
        {
            get { return _data["SomeProp"]; }
            set { _data["SomeProp"] = value; }
        }
    }
}