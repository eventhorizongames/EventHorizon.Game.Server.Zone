using Xunit.Abstractions;

namespace EventHorizon.Game.Server.Zone.Tests.Base
{
    public class TestFixtureBase
    {
        protected readonly ITestOutputHelper _testOutputHelper;

        public TestFixtureBase(
            ITestOutputHelper testOutputHelper
        )
        {
            _testOutputHelper = testOutputHelper;
        }
    }
}
